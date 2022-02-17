import './IncomingAcceptanceRequest.css'
import ProfilePreview from "../ProfilePreview/ProfilePreview";
import React,{useState} from "react";
import {requestAcceptance} from "../../services/ConnectionRequestService";
import {toast} from "react-toastify";
import Login from "../Login/Login";
import Popup from "reactjs-popup";
import TagsInput from "react-tagsinput";
import RequestAcceptanceDTO from "../../model/ConnectionRequest/RequestAcceptanceDTO";
import { useTranslation } from 'react-i18next';



export default function IncomingAcceptanceRequest(props){
    
    const{request} = props
    
    

    const { t } = useTranslation()
    
    const[state,changeState] = useState({
        connStrength: 0,
        tags: []
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

    function handleError(message){
        toast.error(message,{
            position: "top-center",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            className:"Toastify__progress-bar--error"
        })
    }

    function handleRejectAcceptance() {
        const rejection = new RequestAcceptanceDTO(false, -1, null)
        requestAcceptance(request.id, rejection)
            .then(() => toast.success(t('incacceptance.success'),{
                position: "top-center",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                className:"Toastify__progress-bar--success"
            })
        )
        .catch(() => handleError(t('incacceptance.failnetwork')))
    }


    const handleSubmit = (event) => {
        event.preventDefault()
        
        if (state.connStrength>100|| state.connStrength<1) {
            handleError(t('editconnections.invalidconnstrength'))
            return
        }
        
        const data = new RequestAcceptanceDTO(true, state.connStrength, state.tags)
        
        requestAcceptance(request.id, data)
            .then( ()=>toast.success(t('incacceptance.success'),{
                position: "top-center",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                className:"Toastify__progress-bar--success"
            }) )
            .catch(() => handleError(t('incacceptance.failnetwork')))
    }

   

    {/*
                    <p>{t('incacceptance.msg')} {request.messageOrigToDest} </p>*/}
    
   
    return (
        <div className="incomingRequest">

            <div className="requestInfo">
                <div className="Preview">
                    <ProfilePreview user={request.oUser}/>
                    <h4>{request.oUser.name} </h4>
                    <h3>{t('incacceptance.dirconnection')}</h3>
                </div>
                
                <div className="information">
                    {request.messageInterToDest &&<p>{t('incacceptance.msgInter')} {request.messageInterToDest} </p>}
                    <p>{t('incacceptance.msgOrig')} {request.messageOrigToDest} </p>
                </div>

                <div className="requestAction">
                    <Popup trigger={<button>{t('incacceptance.accept')}</button>} modal
                           nested>
                        {close=>(
                            <div className="modal">
                                <button className="close" onClick={close}>
                                    &times;
                                </button>
                                <div className="header">{t('incacceptance.header')}</div>

                                <form onSubmit={handleSubmit} className="IncomingDirectRequest">
                                    <div className="form__group field">
                                        <input
                                            type="number"
                                            placeholder="Connection Strength"
                                            onChange={handleChange}
                                            name="connStrength"
                                            id="connStrength"
                                            value={state.connStrength}
                                            className="form__field"
                                            min={0} max={100}
                                            required
                                        />
                                        <label htmlFor="connStrength" className="form__label">{t('directrequest.connstrength')}</label>
                                    </div>

                                    <TagsInput value={state.tags} onChange={(tags) => changeState({ ...state, tags })} addKeys={[9,13,32]} />
                                    <input type="submit" value={t('incacceptance.accept')} />
                                </form>
                            </div>
                        )}
                    </Popup>

                    <button className="reject" onClick={handleRejectAcceptance}>{t('incacceptance.reject')}</button>
                </div>
                
                
                
            </div>

            
            
            
            
            
        </div>
    )
    
    
}