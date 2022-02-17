import Popup from "reactjs-popup";
import React, {useState} from "react";
import {useTranslation} from "react-i18next";
import {userNetworkSize} from "../../services/UserNetworkService";
import {failedNotification} from "../../utils/ToastContainerUtils";
import "./NetworkSize.css"
export default function NetworkSize({userId}){

    const { t } = useTranslation()

    const [state, setState] = useState({
        level: 1,
        size:undefined
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

    const handleSubmit = (event) => {
        event.preventDefault()
        userNetworkSize(userId, state.level)
            .then(res => setState({ ...state, size: res.data.value }))
            .catch((err) => failedNotification(err.response.data))
    }
    
    return(
        <Popup trigger={<button className="view-profile-action">{t('networksize.title')}</button>} modal
               nested>
            {close => (
                <div className="modal">
                    <button className="close" onClick={close}>
                        &times;
                    </button>
                    <div className="header">{t('networksize.title')}</div>
                    
                    <div className="network-size-main-container">
                        <div className="network-size-input-container">
                            <form className="modal" onSubmit={handleSubmit}>

                                <div className="form__group field">
                                    <input
                                        type="number"
                                        min={1} max={3}
                                        placeholder="Level"
                                        onChange={handleChange}
                                        name="level"
                                        id="level"
                                        value={state.level}
                                        required
                                        className="form__field"
                                    />
                                    <label htmlFor="level" className="form__label">{t('usrntw.maxlevel')}</label>
                                </div>

                                <input className="network-size-submit-button" type="submit" value={t('usrntw.submit')} />

                            </form>
                        </div>
                        {state.size!==undefined&&
                        <div className="network-size-results-container">
                            <p>{t('networksie.result')} <span className="network-size-result">{state.size}</span></p>
                        </div>
                        }
                        
                    </div>
                    

                </div>
            )}
        </Popup>
    )
}