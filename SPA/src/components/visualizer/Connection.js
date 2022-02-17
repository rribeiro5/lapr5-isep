import React, { useRef } from 'react'
import * as THREE from 'three'

export default (props) => {
    const mesh = useRef()

    const direction = new THREE.Vector3().subVectors(props.pointY, props.pointX)
    const arrow = new THREE.ArrowHelper( direction.clone().normalize(), props.pointY )

    // Thickness = (MaxThickness - MinThickness) * (connectionStrength / 100) + MinThickness
    const maxThickness = 2.5
    const minThickness = 0.2
    const radius = ((maxThickness - minThickness) * (props.connectionStrength / 100) + minThickness) / 2 

    return (
        <mesh
            castShadow = {true}
            receiveShadow = {true}
            ref={mesh}
            rotation={arrow.rotation.clone()}
            position={new THREE.Vector3().addVectors( props.pointX, direction.clone().multiplyScalar(0.5) )}
        >
            <cylinderGeometry args={[radius,radius,direction.length(),30,30]} />
            <meshPhongMaterial color={props.inPath ? "indigo" : "hotpink"} />
        </mesh>
    )
}