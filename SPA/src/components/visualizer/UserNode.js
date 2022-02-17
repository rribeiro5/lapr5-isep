import React, { Suspense, useEffect, useRef, useState } from 'react'
import {Billboard, Image, Text} from "@react-three/drei";
import {useLoader} from "@react-three/fiber";
import {OBJLoader} from "three/examples/jsm/loaders/OBJLoader";
import {MTLLoader} from "three/examples/jsm/loaders/MTLLoader";
import JoyfulSVG from "../../assets/Emotions/Joyful/joyful.svg"
import DistressedSVG from "../../assets/Emotions/Distressed/Distressed.svg"
import HopefulSVG from "../../assets/Emotions/Hopeful/Hopeful.svg"
import FearfulSVG from "../../assets/Emotions/Fearful/Fearful.svg"
import ProudSVG from "../../assets/Emotions/Proud/Proud.svg"
import RelieveSVG from "../../assets/Emotions/Relieve/Relieve.svg"
import RemorsefulSVG from "../../assets/Emotions/Remorseful/Remorseful.svg"
import AngrySVG from "../../assets/Emotions/Angry/Angry.svg"
import DisappointedSVG from "../../assets/Emotions/Disappointed/Disappointed.svg"
import GratefulSVG from "../../assets/Emotions/Grateful/Grateful.svg";
import DefaultEmotionSVG from "../../assets/Emotions/DefaultEmotion/DefaultEmotion.svg"

const colors = ["blue", "green", "crimson", "mediumaquamarine", "mediumpurple", "lightsalmon"]

export default (props) => {
    const mesh = useRef()
    const materials = useLoader(MTLLoader, "3D_Models/Humano_01Business_01_30K.mtl")
    const obj = useLoader(OBJLoader, "3D_Models/Humano_01Business_01_30K.obj", (loader) => {
        materials.preload();
        loader.setMaterials(materials);
    });
    
    const [hovered, setHover] = useState(false)
    
    const currentEmotion = getEmotion()
    
    function getEmotion (){
        
        switch (props.emotionalState){
            case "Joyful":
                return JoyfulSVG
            case "Distressed":
                return  DistressedSVG
            case "Hopeful":
                return  HopefulSVG
            case "Fearful":
                return  FearfulSVG
            case "Relieve":
                return  RelieveSVG
            case "Disappointed":
                return  DisappointedSVG
            case "Proud":
                return  ProudSVG
            case "Remorseful":
                return  RemorsefulSVG
            case "Grateful":
                return  GratefulSVG
            case "Angry":
                return  AngrySVG
            default:
                return  DefaultEmotionSVG
        }
    }
    
    const getColor = (userLevel) => {
        while (userLevel >= colors.length) {
            userLevel -= colors.length
        }
        return colors[userLevel]
    }

    return (
 
        <mesh
            castShadow={true}
            receiveShadow={true}
            ref={mesh}
            position={props.position}
            scale={hovered ? 1.1 : 1}
            onPointerOver={(_) => setHover(true)}
            onPointerOut={(_) => setHover(false)}
            onClick={(_) => {
                if (props.selectNode !== undefined)
                    props.selectNode(props)
            }}
        >

            <sphereGeometry args={[2, 32, 16]} />
            <meshPhongMaterial color={hovered ? 'orange'  : getColor(props.userLevel) }  />
            <Billboard
                follow={true}
                lockX={false}
                lockY={false}
                lockZ={false}
                position={hovered ? [0, 12, 0] :[0, 3, 0]}
                args={[44, 30]}>
                <Text fontSize={1} outlineWidth={'5%'} outlineColor="#000000" outlineOpacity={1}>
                    {props.email}
                </Text>
                
            </Billboard>
            {props.showTip && 
            <Billboard
                visible={hovered}
                follow={true}
                lockX={false}
                lockY={false}
                lockZ={false}
                position={[0, 3, -3]} 
                args={[44, 30]}
                scale={hovered ? 0.9 : 0}>
                {/*validAvatar ? <Suspense fallback={null}><Image url={props.avatar} position={[0, 5, 0]} scale={2.1} /></Suspense> : null*/}
                <Image url={currentEmotion} position={[0, 5.5, 0]} scale={4} visible={hovered}  />
                <Text
                    fontSize={0.8}
                    outlineWidth={'5%'}
                    outlineColor="#000000"
                    outlineOpacity={1}
                    position={[0, 3, 0]}>
                    {props.name}
                </Text>
                <Text
                    fontSize={0.6}
                    outlineWidth={'5%'}
                    outlineColor="#000000"
                    outlineOpacity={1}
                    position={[0,2,0]}>
                    {props.interestTags.join(", ")}
                </Text>
                <Text
                    fontSize={0.6}
                    outlineWidth={'5%'}
                    outlineColor="#000000"
                    outlineOpacity={1}
                    position={[0,1,0]}>
                    {props.emotionalState? `Feeling ${props.emotionalState}`: null}
                </Text>
            </Billboard>}
            {props.showTip &&
            <primitive 
                object={obj.clone()} 
                position={[0, 3, 4]} 
                scale={hovered ? 0.03 : 0} 
                visible={hovered}
            />
            }
        </mesh>
        
    )
}
