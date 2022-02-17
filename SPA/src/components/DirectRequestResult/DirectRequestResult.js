import ProfilePreview from "../ProfilePreview/ProfilePreview";
import DirectConnectionRequest from "../DirectConnectionRequest/DirectConnectionRequest";
import React, {useContext, useEffect, useState} from "react";
import {Context} from "../../context/loggedUser";

import {getConnectionsOfUser} from "../../services/ConnectionService";
import {useTranslation} from "react-i18next";

export default function DirectRequestResult({user}){
    const {loggedUser} = useContext(Context)
    const [friend,setFriendState] = useState(false)

    const { t } = useTranslation()
    
    useEffect(()=>{
        getConnectionsOfUser(loggedUser.id)
            .then(res =>{
                
                res.data.connections.forEach(connection =>{
                    if(connection.dUser.value === user.id || connection.oUser.value === user.id){
                        setFriendState(true)
                    } 
                })
            })
    },[])
    
    
    
    return(
        <div  className="sugestion">
            <div className="user-info">
                <ProfilePreview  user={user} />
                <h3>{user.name}</h3>
            </div>
            
            <div className="direct-request-result-action">
                {!friend?<DirectConnectionRequest orig={loggedUser.id} dest={user} />:<span className="friend-span">{t("directRequestResult.friend")}</span>}
            </div>
        </div>
    )
}