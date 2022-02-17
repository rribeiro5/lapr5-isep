
import {pendingRequests,requestAcceptance , updateApprovalState} from "../../services/ConnectionRequestService";
import React, {useState, useEffect, useContext} from "react"
import {toast} from "react-toastify";
import 'reactjs-popup/dist/index.css';
import './IncomingRequests.css'
import IncomingAcceptanceRequest from "../IncomingAcceptanceRequest/IncomingAcceptanceRequest";
import IncomingAprovalRequest from "../IncomingAprovalRequest/IncomingAprovalRequest";
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from "react-i18next";

export default function IncomingRequests(props){
    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()
    
    const currentUserId = loggedUser.id;
    
    const[state,setState]= useState({
        pendingRequests:[]
    })
    
    
    // buscar e criar as pending requests
    useEffect(()=>{
            props.nameCurrentComponent(t('increquests.title'))
            pendingRequests(currentUserId)
                .then(res => setState(prevState=>{
                    
                    let array = []
                    for(let index in res.data){
                        let object = res.data[index]
                        
                        if(object.dUser.id===currentUserId){
                            array.push(<IncomingAcceptanceRequest key={object.id} request={object}/> )
                        }else{
                            array.push(<IncomingAprovalRequest key={object.id} request={object}/> )
                        }
                    }
                    
                    return{
                        ...prevState,
                        pendingRequests: array
                    }
                }))
                .catch(e=>failedNotification(e.response))
        }
        ,[state.pendingRequests])
    
    
    return(
        <div className="IncomingRequests">
            {state.pendingRequests}
        </div>
    )
}