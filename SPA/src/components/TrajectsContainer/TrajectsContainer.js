import React, {useEffect, useState} from 'react';
import {getAllUsers} from "../../services/UserService";
import StrongestPath from "../StrongestPath/StrongestPath";
import Popup from "reactjs-popup";
import './TrajectsContainer.css'
import SafestPath from "../SafestPath/SafestPath";
import ShortestPath from "../ShortestPath/ShortestPath";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';
import NLimitPath from '../NLimitPath/NLimitPath';
import * as ApiAiService from '../../services/ApiAIService'

export default function TrajectsContainer(props){

    const { t } = useTranslation()

    const defaultOption = <option key="-1" value="-1" >{t('introductionreq.selectdefault')}</option>
    
    const [state,changeState] = useState({
        origUser:"-1",
        destUser:"-1",
        users:[]
    })


    function handleChange(event){
        const {name,value} = event.target
        changeState(prevData=>{
                return {
                    ...prevData,
                    [name]:value
                }
            }
        )
    }
    
    // obtain users
    useEffect(()=>{
        props.nameCurrentComponent(t('trajects.title'))
        getAllUsers().then(res => {
            changeState(prevState => {
                return {
                    ...prevState,
                    users: [defaultOption, res.data.map(user => <option key={user.id} value={user.email}>{user.email}</option>)]
                }
            })
            
        }).catch(err => failedNotification(err.response.data))
        
    },[])
    
    
    
    return(
        <div className="TrajectsContainer">
            
            <div className="selectContainer">
                <label htmlFor="originUserSelect">{t('trajects.origuser')}</label>
                <select
                    id="originUserSelect"
                    value={state.origUser}
                    onChange={handleChange}
                    name="origUser"
                >
                    {state.users}
                </select>
            </div>

            <div className="selectContainer">
                <label htmlFor="destUserSelect">
                    {t('trajects.destuser')}</label>
                
                <select
                    id="destUserSelect"
                    value={state.destUser}
                    onChange={handleChange}
                    name="destUser"
                >
                    {state.users}
                </select>
            </div>
            
            <div className="buttons-container">
                <Popup trigger={<button>{t('trajects.shorter')}</button>} modal
                       nested>
                    {close=>(
                        <div className="modal">
                            <button className="close" onClick={close}>
                                &times;
                            </button>
                            <div className="header">{t('trajects.shortestpath')}</div>
                            <ShortestPath {...state} />
                        </div>
                    )}
                </Popup>
                
                <Popup trigger={<button>{t('trajects.stronger')}</button>} modal
                       nested>
                    {close=>(
                        <div className="modal">
                            <button className="close" onClick={close}>
                                &times;
                            </button>
                            <div className="header">{t('trajects.strongestpath')}</div>
                            <StrongestPath {...state} />
                        </div>
                    )}
                </Popup>
                
                <Popup trigger={<button>{t('trajects.safest')}</button>} modal
                       nested>
                    {close=>(
                        <div className="modal">
                            <button className="close" onClick={close}>
                                &times;
                            </button>
                            <div className="header">{t('trajects.safestpath')}</div>
                            <SafestPath {...state} />
                        </div>
                    )}
                </Popup>
                
            </div>

            <div className="buttons-container">
                <NLimitPath btn="DFS" header="DFS" origUser={state.origUser} destUser={state.destUser} 
                    handlerFLig={ApiAiService.getDFSFLig} handlerFRel={ApiAiService.getDFSFRel}
                    handlerFLigEmotion={ApiAiService.getDFSFLigEmotion} handlerFRelEmotion={ApiAiService.getDFSFRelEmotion} />
                <NLimitPath btn="BestFirst" header="BestFirst" origUser={state.origUser} destUser={state.destUser} 
                    handlerFLig={ApiAiService.getBestFirstFLig} handlerFRel={ApiAiService.getBestFirstFRel}
                    handlerFLigEmotion={ApiAiService.getBestFirstFLigEmotion} handlerFRelEmotion={ApiAiService.getBestFirstFRelEmotion} />
                <NLimitPath btn="A*" header="A*" origUser={state.origUser} destUser={state.destUser} 
                    handlerFLig={ApiAiService.getAStarFLig} handlerFRel={ApiAiService.getAStarFRel}
                    handlerFLigEmotion={ApiAiService.getAStarFLigEmotion} handlerFRelEmotion={ApiAiService.getAStarFRelEmotion} />
            </div>
            
        </div>
    )
}