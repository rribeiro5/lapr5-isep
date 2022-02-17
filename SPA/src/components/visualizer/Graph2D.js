import {useContext, useEffect, useState, Suspense} from 'react'
import { Canvas } from '@react-three/fiber'
import UserNode from './UserNode'
import { OrbitControls } from "@react-three/drei";
import Connection from './Connection'
import {getUserNetwork} from "../../services/UserService";
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import {generateGraph} from "../../utils/GenerateGraph";


export default () => {

    const {loggedUser} = useContext(Context)
    
    // todo improvement 
    const DEFAULT_LEVEL = 100
    
    const [state, setState] = useState({
        network: undefined, // Response of request
        userNodes: [], // Generated UserNodeDTOs
        connections: [], // Generated GraphConnectionDTOs
    })

    useEffect(() => {
        getUserNetwork(loggedUser.id,DEFAULT_LEVEL).then( response => setState( prevState => {
            return{
                    ...prevState,
                    network: response.data
                }
            }))
            .catch((err) => failedNotification(err.response.data))
    }, [])
   
    useEffect(()=>{
        const {network} = state
        if (network !== undefined) {
            const userNodes = []
            const connections = []
            generateGraph({ ...network }, [0,0,0], 0, 2 * Math.PI, userNodes, connections)
            setState({ ...state, userNodes, connections })
        }
    },[state.network])

    return (
        <Canvas orthographic camera={{zoom: 3, position: [0, 35, 0]}}>
            <Suspense fallback={null}>
            <ambientLight intensity={2} />
            <OrbitControls  enablePan={true} enableZoom={true} enableRotate={false} minZoom={1} maxZoom={12} />
            {state.userNodes.map( (user) => <UserNode key={user.id} {...user} showTip={false}/>)}
            {state.connections.map((conn,i) => <Connection key={i} {...conn} />)}
            </Suspense>
        </Canvas>
    )
}