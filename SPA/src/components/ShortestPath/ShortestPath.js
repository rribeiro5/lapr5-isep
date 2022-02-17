import React,{useState,useEffect} from 'react';
import {
    getShortestTraject
} from "../../services/ApiAIService";
import {toast} from "react-toastify";
import TrajectViewer from "../TrajectViewer/TrajectViewer";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';



export default function ShortestPath(props){
    const{origUser,destUser} = props

    const { t } = useTranslation()

    const [state,changeState] = useState({
        caminho:[]
    })
    
    useEffect(()=>{

        obtainShortestPath()
        
    },[])
    
    
    function obtainShortestPath(){
        
        if(origUser==="-1" || destUser ==="-1"){
            failedNotification(t('directrequest.noreqselected'))
            return;
        }
        
        getShortestTraject(origUser,destUser)
            .then(res => {
                const {caminho} = res.data
                console.log(res.data)
                changeState( prevState => {
                    return{
                        ...prevState,
                        caminho
                    }
                })
            })
            .catch(() => failedNotification(t('safest.errstrength')))
    }
    
    return(
        <div className="SafestPath">
            <TrajectViewer {...state} />
        </div>
    )

}