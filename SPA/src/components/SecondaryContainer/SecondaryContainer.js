import Graph2D from "../visualizer/Graph2D";
import React, {useContext, useEffect, useState} from "react";
import './SecondaryContainer.css'
import UserSugestionSidebar from "../UserSugestionSidebar/UserSugestionSidebar";
import {getSuggestedUser} from "../../services/ApiAIService";
import {getDTOSuggestedUser} from "../../services/UserService";
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from "react-i18next";
import SocialNetworkInfo from "../SocialNetworkInfo/SocialNetworkInfo";

export default function SecondaryContainer(){

    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()
    
    const[state,changeState] = useState({
        users:[],
        dto:[]
    })
    
    // buscar sugestoes
    useEffect( ()=>{
        
        
        getSuggestedUser(loggedUser.id)
            .then(res => {
                changeState(prevState => {
                    return{
                        ...prevState,
                        users:res.data.ids
                    }
                })    
            })
            .catch(err => failedNotification(err.response))
    },[])
    
    
    // buscar dtos
    useEffect( ()=>{
            let newDto = []
            for(let index in state.users){
                let id = state.users[index]
                
                getDTOSuggestedUser(id).then(res => {
                    //console.log(res.data)
                    changeState(prevState => {
                        return{
                            ...prevState,
                            dto:[...prevState.dto,<UserSugestionSidebar key={res.data.id} user={res.data}/>]
                        }
                    })
                    
                })
            }
            
            changeState(prevState => {
                return{
                    ...prevState,
                    dto:[...newDto]
                }
            })
        
    },[state.users])
    
    
    return(
        <div className="secondaryContainer">
            <div className="sugestions" >
                <h3>{t('rigthbar.suggestions')}</h3>
                {state.dto}
            </div>
            
            <div className="miniMap" >
                <h2>{t('rigthbar.minimap')}</h2>
                <div>
                    <Graph2D  />
                </div>
            </div>

            <SocialNetworkInfo />
        </div>
    )
}