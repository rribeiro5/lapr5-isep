
import Popup from "reactjs-popup";
import TagsInput from "react-tagsinput";
import React, {useContext, useState} from "react";
import './DirectConnectionRequest.css'
import {createDirectRequest} from "../../services/ConnectionRequestService";
import CreacteDirectRequestDTO from "../../model/ConnectionRequest/CreateDirectRequestDTO";
import {Context} from "../../context/loggedUser";
import {failedNotification,successNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from "react-i18next";

export default function  DirectConnectionRequest(props){

    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()
    
    const {dest} = props
    
    const[state,setState] = useState({
        MessageOrigToDest: "Teste",
        connectionStrength: 1,
        Tags: []
    })
    
    function handleChange(event){
        const {name,value} = event.target
        setState(prevData=>{
                return {
                    ...prevData,
                    [name]:value
                }
            }
        )
    }
    

    const handleSubmit = (event) =>{
        event.preventDefault();

        if(dest === undefined){
            failedNotification(t('directrequest.noreqselected'))
            return
        }
        
        if(state.messageOrigToDest === ""){
            failedNotification(t('directrequest.failmsgdest'))
            return
        }
        
        if(state.Tags.length === 0){
            failedNotification(t('directrequest.failnotags'))
            return
        }
        
        const {MessageOrigToDest,connectionStrength,Tags} = state
        const body = new CreacteDirectRequestDTO(loggedUser.id,dest.id,MessageOrigToDest,connectionStrength,Tags)
        
        createDirectRequest(body)
            .then(()=> successNotification(t('directrequest.success')))
            .catch(err=>failedNotification(err.response.data))
    }




    return(
        <Popup trigger={<button className="DirectConnectionRequestButton">{t('directrequest.connection')}</button>} modal
               nested>
            {close=>(
                <div className="modal">
                    <button className="close" onClick={close}>
                        &times;
                    </button>
                    <div className="header">{t('directrequest.header')}</div>
                    <div className="DirectConnectionRequestContainer">

                        <form onSubmit={handleSubmit} className="IndirectConnectionForm" >
                            

                            <div className="form__group field">
                                <input
                                    type="text"
                                    placeholder="Message to Destiny User"
                                    onChange={handleChange}
                                    name="MessageOrigToDest"
                                    id="MessageOrigToDest"
                                    value={state.messageOrigToDest}
                                    className="form__field"
                                />
                                <label htmlFor="messageOrigToDest" className="form__label">{t('directrequest.msgdest')}</label>
                            </div>
                            
                            <div className="form__group field">
                                <input
                                    type="number"
                                    min="0"
                                    max="100"
                                    placeholder="Connection Strength"
                                    onChange={handleChange}
                                    name="connectionStrength"
                                    id="connectionStrength"
                                    value={state.connectionStrength}
                                    className="form__field"
                                />
                                <label htmlFor="connectionStrength" className="form__label">{t('directrequest.connstrength')}</label>
                            </div>

                            <TagsInput value={state.Tags} addKeys={[9,13,32]}  onChange={(Tags)=>setState(prevData => {return {...prevData,Tags}})} required />

                            <input type="submit" value={t('directrequest.submit')} />
                            

                        </form>
                        
                    </div>
                    
                </div>
            )}
        </Popup>
    )
    
    
}