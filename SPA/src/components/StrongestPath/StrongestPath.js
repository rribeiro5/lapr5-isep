import React,{useState,useEffect} from 'react';
import { getStrongestTrajectUnidirecional,getStrongestTrajectBidirecional} from "../../services/ApiAIService";
import {toast} from "react-toastify";
import TrajectViewer from "../TrajectViewer/TrajectViewer";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';



export default function StrongestPath(props){
    const{origUser,destUser} = props

    const { t } = useTranslation()

    const [state,changeState] = useState({
        caminho:[],
        forcaResultante:""
    })
    
    function validateData(){
        if(origUser==="-1" || destUser ==="-1"){
            failedNotification(t('directrequest.noreqselected'))
            return false;
        }
        return true;
    }
    
    function obtainUnidirecionalPath(event){
        event.preventDefault()

       if(!validateData()) return ;

        getStrongestTrajectUnidirecional(origUser,destUser)
            .then(res => {
                const {forcaResultante,caminho} = res.data
                changeState( prevState => {
                    return{
                        ...prevState,
                        forcaResultante,
                        caminho
                    }
                })
            })
            .catch(() => failedNotification(t('safest.errstrength')))
    }

    function obtainBidirecionalPath(event){
        event.preventDefault()

        if(!validateData()) return ;
        
        getStrongestTrajectBidirecional(origUser,destUser)
            .then(res => {
                const {forcaResultante,caminho} = res.data
                changeState( prevState => {
                    return{
                        ...prevState,
                        forcaResultante,
                        caminho
                    }
                })
            })
            .catch(() => failedNotification(t('safest.errstrength')))
    }
    
    return(
        <div className="SafestPath">

            
            <div className="buttons-container">
                <button onClick={obtainUnidirecionalPath}>{t('trajects.uni')}</button>
                <button onClick={obtainBidirecionalPath}>{t('trajects.bi')}</button>
            </div>

            <TrajectViewer {...state} />

        </div>
    )
    
}