import React, {useEffect, useState} from "react";
import {userNetworkStrength} from "../../services/UserNetworkService";
import {failedNotification} from "../../utils/ToastContainerUtils";
import {useTranslation} from "react-i18next";
import "./NetworkStrength.css"

export default function NetworkStrength({userId}){
    
    const[strength,setStrength] = useState(0);

    const { t } = useTranslation()
    
    useEffect(()=>{
        if (!!userId) {
            userNetworkStrength(userId)
            .then(res=>setStrength(res.data.value))
            .catch(()=>failedNotification("Network Error"))
        }
    },[userId])
    
    return(
        <span className="network-strength-points">{t('profilePreview.strength')}{strength} </span>
    )
}