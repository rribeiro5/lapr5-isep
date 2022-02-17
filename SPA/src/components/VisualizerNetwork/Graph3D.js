import {useContext, useEffect, useRef, useState,Suspense} from 'react'
import {Canvas, useFrame, useThree} from '@react-three/fiber'
import UserNode from '../visualizer/UserNode'
import { OrbitControls,PerspectiveCamera,FlyControls , ContactShadows , Plane,useHelper,useDepthBuffer,Environment} from '@react-three/drei'
import DatGui, {DatSelect} from "react-dat-gui";
import {  SpotLightHelper, PointLightHelper,Vector3 } from "three"

import * as THREE from "three";

 import Connection from '../visualizer/Connection'
import {getUserNetwork} from "../../services/UserService";
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import './Graph3D.css'
import { MdHome } from 'react-icons/md'
import { Link } from 'react-router-dom'
import { getShortestTraject } from '../../services/ApiAIService'
import {useTranslation} from "react-i18next";
import {generateGraph} from "../../utils/GenerateGraph";
import {REF} from "three/examples/jsm/loaders/ifc/web-ifc-api";


export default function Graph3D()  {
    const {loggedUser} = useContext(Context)
    const { t } = useTranslation()
    // todo improvement 
    const DEFAULT_LEVEL = 100
    
    const [state, setState] = useState({
        network: undefined, // Response of request
        userNodes: [], // Generated UserNodeDTOs
        connections: [], // Generated GraphConnectionDTOs
        xType: "Orbit Controls",
        selected: undefined,
        selectedEmail:""
    })


    useEffect(() => {
        getUserNetwork(loggedUser.id,DEFAULT_LEVEL).then( response => setState( prevState => {
            return{
                ...prevState,
                network: response.data,
            }
        }))
            .catch((err) => failedNotification(err.response.data))
    }, [])

    useEffect(()=>{
        const {network} = state
        if (network !== undefined) {
            const userNodes = []
            const connections = []
            generateGraph({ ...network }, [0,0,0], 0, 2 * Math.PI, userNodes, connections,true)
            setState({ ...state, userNodes, connections })
        }
    },[state.network])

    useEffect(() => {
        cleanPath()
        if (state.selected !== undefined) {
            setPath(state.selected)
        }
    }, [state.selected])

    const setPath = (user) => {
        if (user.email !== loggedUser.email) {
            getShortestTraject(loggedUser.email, user.email)
                .then(res => {
                    const { caminho } = res.data
                    const pairs = []
                    for (let i = 1; i < caminho.length; i++) {
                        pairs.push({ orig: caminho[i - 1], dest: caminho[i] })
                    }
                    const isInPairs = (orig, dest, pairs) => {
                        return pairs.some(pair => (pair.orig === orig.email && pair.dest === dest.email) || (pair.orig === dest.email && pair.dest === orig.email))
                    }
                    setState({ ...state, connections: state.connections.map(conn => { return { ...conn, inPath: isInPairs(conn.origUser, conn.destUser, pairs) } }) })
                })
                .catch(err => console.error(err))
        }
    }

    const cleanPath = () => {
        setState({ ...state, connections: state.connections.map(conn => { return { ...conn, inPath: false } }) })
    }

    function handleUpdate (newData){
        setState(prevData => {
            return {
                ...prevData,
                ...newData
            }    
    })}

    const selectNode = (user) => {
        setState({ ...state, selected: user })
    }


    function handleDestEmailUpdate(e){
        e.preventDefault()
        const {value}=e.target
        setState(prevState => ({...prevState,"selectedEmail":value}))
    }
    
    function handleGetPath(e){
        e.preventDefault()
        const user=state.userNodes.find(u=>u.email===state.selectedEmail.toString())
        if(user===null || user===undefined){
            failedNotification(t('graph3d.userNotFound')+state.selectedEmail)
            return
        }
        setPath(user)
    }
    
    function handleResetButton(e){
        e.preventDefault()
        cleanPath()
        setState(prevState => ({...prevState,"selectedEmail":""}))
    }
    
    const pointLineDistance3d=(position,pointX,pointY)=>{
        const target=new THREE.Vector3()
        const v0=new THREE.Vector3(position.x,position.y,position.z)
        const v1=new THREE.Vector3(pointX[0],pointX[1],pointX[2])
        const v2=new THREE.Vector3(pointY.x,pointY.y,pointY.z)
        const line=new THREE.Line3(v1,v2)
        return line.closestPointToPoint(v0,true,target).distanceTo(v0)
    }
    
    const detectCollision=(position)=>{
        for(let node of state.userNodes){
            const x=Math.pow(position.x-node.position[0],2)
            const y=Math.pow(position.y-node.position[1],2)
            const z=Math.pow(position.z-node.position[2],2)
            const distanceToNode = Math.sqrt(x+y+z)
            
            if(distanceToNode< 8){
                return true
            }
        }
        for(let conn of state.connections){
            const distanceToNode=pointLineDistance3d(position,conn.pointX,conn.pointY)
            if(distanceToNode< 8){
                return true
           }
        }
        return false
    }
        
    let oldPosition={
        x:0,
        y:35,
        z:0
    }
    let position
    const Collision=()=>{
        useFrame(()=>{
            const collision=detectCollision(position)
            if(collision){
                position.x=oldPosition.x
                position.y=oldPosition.y
                position.z=oldPosition.z
            }else{
                oldPosition.x=position.x
                oldPosition.y=position.y
                oldPosition.z=position.z
            }
        })
        return null
    }

    function Lights() {

        
        const directionLight1 = useRef()
        const directionLight2 = useRef()
        
        
        //useHelper(directionLight1, PointLightHelper, 0.5, "hotpink")
        //useHelper(directionLight2, PointLightHelper, 0.5, "hotpink")
        
        
        return (
            <>
                <ambientLight intensity={2} />
                <pointLight ref={directionLight1} castShadow={true} intensity={2}  position={[15, 20, 10]} />
                <pointLight ref={directionLight2} castShadow={true} intensity={2}  position={[-15, 20, -10]} />
                
            </>
        )
    }
    const spotLight = useRef()
    const currentCamera = useRef();
    
    return (
        <div className="graph-visualizer-container">
            <Link to="/" className="back-btn">
                <MdHome fontSize="15pt" />
            </Link>
            <DatGui data={state} onUpdate={handleUpdate}>
                <DatSelect
                    path="xType"
                    label="Controls Type"
                    options={["Orbit Controls", "Fly Controls"]}
                />
            </DatGui>
            <div
            className="email-div">
                <input
                    className="email-input"
                    type="text"
                    placeholder={t('graph3d.getPathInputText')}
                    onChange={handleDestEmailUpdate}
                    value={state.selectedEmail}
                />
                <button
                    className="search-path-button"
                    onClick={handleGetPath}
                >
                    {t('graph3d.getPathByEmailButton')}
                </button>
                <button
                    className="search-path-button"
                    onClick={handleResetButton}
                >
                    {t('graph3d.getPathByEmailReset')}
                    
                </button>
            </div>
            <Canvas shadows shadowMap>
                <Suspense fallback={null}>
                <Lights />
                <Collision />
                
                <PerspectiveCamera castShadow={true} ref={currentCamera} makeDefault position={[0,35,0]} onUpdate={(c) => {
                    position=c.position
                    if(spotLight.current){
                        spotLight.current.target = c
                    }
                }} >
                    <spotLight ref={spotLight} color="#ffffff"  target={currentCamera.current}  castShadow={true}  intensity={1}  angle={0.5}  position={[0, 0, 1]}  decay={0} distance={30}  />
                </PerspectiveCamera>
                
                    {state.userNodes.map( (user) => <UserNode key={user.id} {...user} selectNode={selectNode} showTip={true}/>)}
                    {state.connections.map((conn,i) => <Connection key={i} {...conn} />)}
                    {state.xType === "Orbit Controls" ? <OrbitControls maxDistance={50} minDistance={10} /> :
                        <FlyControls movementSpeed={30} rollSpeed={0.5} autoFoward={false} dragToLook={true}/> }

            </Suspense>   
            </Canvas>
        </div>
    )
}