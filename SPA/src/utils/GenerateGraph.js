import GraphConnectionDTO from "../model/Connection/GraphConnectionDTO";
import * as THREE from "three";
import UserNodeDTO from "../model/User/UserNodeDTO";

const betweenZeroAnd2Pi = (angle) => {
    while (angle >= 2 * Math.PI) {
        angle -= 2 * Math.PI
    }
    while (angle < 0) {
        angle += 2 * Math.PI
    }
    return angle
}


const getCoordinates=(dist,connAngle,position,is3d=false)=>{
    const newX = dist * Math.cos(connAngle) + position[0]
    const newZ = dist * Math.sin(connAngle) + position[2]
    if(is3d){
        return [newX,(Math.random() * 9 + 1),newZ]
    }
    return [newX,position[1],newZ]
}

const createGraphConnectionDTO=(user,connection,position,connPosition,dest)=>{
    if(dest===null || dest === undefined){
        return new GraphConnectionDTO(user, connection.user, new THREE.Vector3().fromArray(position), new THREE.Vector3().fromArray(connPosition),
            connection.connectionStrength, connection.relationshipStrength, connection.connTags)
    }
    return new GraphConnectionDTO(user, dest, new THREE.Vector3().fromArray(position), new THREE.Vector3().fromArray(dest.position),
        connection.connectionStrength, connection.relationshipStrength, connection.connTags)
}

const getConnectionData=(i,dist,minAngle,maxAngle,numConns,position,is3d=false)=>{
    const connAngle = betweenZeroAnd2Pi(((maxAngle - minAngle) / numConns) * i)
    const connPosition=getCoordinates(dist,connAngle,position,is3d)
    const angleDiff = ((maxAngle - minAngle) / numConns) / 2
    return {connAngle,connPosition,angleDiff}
}

const createConnection=(user,connection,connPosition,userNodes,userConnections,position,connAngle,angleDiff,connections)=>{
    const dest = userNodes.find((element) => element.id === connection.user.id)
    let graphConnection
    if (dest === undefined) {
        graphConnection=createGraphConnectionDTO(user,connection,position,connPosition,null);
        const {id,name, avatar, userLevel,email,interestTags,emotionalState} = connection.user
        const userNodeDest = new UserNodeDTO(id,name, avatar, userLevel,email,interestTags,emotionalState,connPosition,'blue')
        userNodes.push(userNodeDest)
        userConnections.push({user: connection.user, connPosition: connPosition, minAngle: connAngle - angleDiff,maxAngle: connAngle + angleDiff})
    } else {
        graphConnection=createGraphConnectionDTO(user,connection,position,connPosition,dest);
        const destAngle = Math.atan2(dest.position[2] - position[2], dest.position[0] - position[0])
        userConnections.push({user: connection.user, connPosition: dest.position, minAngle: destAngle - angleDiff,maxAngle: destAngle + angleDiff})
    }
    connections.push(graphConnection)
}

const generateConnection=(user,position,minAngle,maxAngle,userNodes,connections,userConnections,is3d=false)=>{
    const dist = 15
    const numConns = user.connections.length
    for (let i = 1; i <= numConns; i++) {
        const connection = user.connections[i - 1]
        const {connAngle,connPosition,angleDiff} = getConnectionData(i,dist,minAngle,maxAngle,numConns,position,is3d)
        createConnection(user,connection,connPosition,userNodes,userConnections,position,connAngle,angleDiff,connections)
    }
}

export const generateGraph = (user,position,minAngle,maxAngle,userNodes,connections,is3d=false) => {
    const {id,name, avatar, userLevel,email,interestTags,emotionalState} = user
    const userNode = new UserNodeDTO(id,name, avatar,userLevel,email,interestTags,emotionalState,position,'blue')
    if (userNodes.every((node) => node.id !== id)) {
        userNodes.push(userNode)
    }
    const userConnections = []
    generateConnection(user,position,minAngle,maxAngle,userNodes,connections,userConnections,is3d)
    for (const usrConn of userConnections) {
        generateGraph(usrConn.user, usrConn.connPosition, usrConn.minAngle, usrConn.maxAngle, userNodes, connections)
    }
}