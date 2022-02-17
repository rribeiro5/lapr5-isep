import React, {useState} from "react";
import {toast} from "react-toastify";
import RequestAcceptanceDTO from "../../model/ConnectionRequest/RequestAcceptanceDTO";
import {requestAcceptance, updateApprovalState} from "../../services/ConnectionRequestService";
import ProfilePreview from "../ProfilePreview/ProfilePreview";
import Popup from "reactjs-popup";
import TagsInput from "react-tagsinput";
import UpdateApprovalStateRequestDTO from "../../model/ConnectionRequest/UpdateApprovalStateRequestDTO";
import { useTranslation } from "react-i18next";


export default function IncomingAprovalRequest(props){
    
    const {request} = props

    const { t } = useTranslation()
    
    const[state,changeState] = useState({
        message:""
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
    

    function handleDisapproveIntroduction() {
        let response = new UpdateApprovalStateRequestDTO(request.id,"",false)
        updateApprovalState(request.id,response)
            .then(() => toast.success(t('incapproval.rejsuccess'),{
                position: "top-center",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                className:"Toastify__progress-bar--success"
            }))
            .catch(()=>handleError(t('incacceptance.failnetwork')))
    }
    
    
    const handleSubmit = (event) => {
        event.preventDefault()

        if (state.message ==="") {
            handleError(t('incapproval.failmsg'))
            return
        }

        const data = new UpdateApprovalStateRequestDTO(request.id, state.message,true)

        updateApprovalState(request.id,data)
            .then(() =>toast.success(t('incapproval.aprsuccess'),{
                position: "top-center",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                className:"Toastify__progress-bar--success"
            }))
            .catch(()=> handleError(t('incacceptance.failnetwork')));
    }
    

    return (
        <div className="incomingRequest">

            <div className="requestInfo">
                <div className="Preview">
                    <ProfilePreview user={request.oUser}/>
                </div>
                <div className="info">
                    <h3>{t('incapproval.aprvconn')}</h3>
                    <h4>{t('incacceptance.requestedby')} {request.oUser.name} </h4>
                    <p>{t('incacceptance.msg')} {request.messageOrigToInter} </p>
                </div>
            </div>

            <div className="requestAction">
                <Popup trigger={<button>{t('incapproval.approve')}</button>} modal
                       nested>
                    {close=>(
                        <div className="modal">
                            <button className="close" onClick={close}>
                                &times;
                            </button>
                            <div className="header">{t('incapproval.aprvconn')}</div>

                            <form onSubmit={handleSubmit} className="IncomingAprovalRequest">
                                <div className="form__group field">
                                    <input
                                        type="text"
                                        placeholder="Message to End User"
                                        onChange={handleChange}
                                        name="message"
                                        id="message"
                                        value={state.message}
                                        className="form__field"
                                        required
                                    />
                                    <label htmlFor="message" className="form__label">{t('incapproval.msgdest')}</label>
                                </div>
                                <input type="submit" value="Approve" />
                            </form>
                        </div>
                    )}
                </Popup>

                <button className="reject" onClick={handleDisapproveIntroduction}>{t('incapproval.disapprove')}</button>
            </div>

        </div>
    )

}