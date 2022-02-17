import React,{useState} from 'react';
import {getSafestTrajectUnidirecional,getSafestTrajectBidirecional} from "../../services/ApiAIService";
import {toast} from "react-toastify";
import TrajectViewer from "../TrajectViewer/TrajectViewer";
import './SafestPath.css'
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';

export default function SafestPath(props){
    const{origUser,destUser} = props

    const { t } = useTranslation()

    const [state,changeState] = useState({
        caminho:[],
        forcaResultante:"",
        minValue:""
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
    
    function validateData(){
        if(origUser==="-1" || destUser ==="-1"){
            failedNotification(t('directrequest.noreqselected'))
            return false;
        }

        if(state.minValue===""|| state.minValue< 0 || state.minValue > 100){
            failedNotification(t('safest.errstrength'))
            return false
        }
        return true
    }

    
    function obtainUnidirecionalPath(event){
        event.preventDefault()
        
        if(!validateData()) return 
        
        getSafestTrajectUnidirecional(origUser,destUser,state.minValue)
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
            .catch(() => failedNotification(t('incacceptance.failnetwork')))
    }

    function obtainBidirecionalPath(event){
        event.preventDefault()

        if(!validateData()) return

        getSafestTrajectBidirecional(origUser,destUser,state.minValue)
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
            .catch(() => failedNotification(t('incacceptance.failnetwork')))
    }
    
    
    return(
        <div className="SafestPath">

            <div className="form__group field">
                <input
                    type="text"
                    placeholder="Minime Value for Connection"
                    onChange={handleChange}
                    name="minValue"
                    id="minValue"
                    value={state.minValue}
                    required
                    className="form__field"
                />
                <label htmlFor="minValue" className="form__label">{t('safest.minimum')}</label>
            </div>
            
            <div className="buttons-container">
                <button onClick={obtainUnidirecionalPath}>{t('trajects.uni')}</button>
                <button onClick={obtainBidirecionalPath}>{t('trajects.bi')}</button>
            </div>
            
            <TrajectViewer {...state} />
           
        </div>
    )

}