import { useState } from "react"
import { useTranslation } from "react-i18next";
import Popup from "reactjs-popup"
import {failedNotification} from "../../utils/ToastContainerUtils";
import TrajectViewer from "../TrajectViewer/TrajectViewer"

import './NLimitPath.css'

export default ({btn,header,origUser,destUser,handlerFLig,handlerFRel,handlerFLigEmotion,handlerFRelEmotion}) => {

    const { t } = useTranslation()

    const [state,changeState] = useState({
        caminho: [],
        forcaResultante:"",
        nlimit: 1,
        emotion: false
    })

    const handleChangeN = (event) => {
        const {value} = event.target
        changeState(prevData=>{
                return {
                    ...prevData,
                    nlimit: value
                }
            }
        )
    }

    const handleCheckbox = () => {
        changeState({ ...state, emotion: !state.emotion })
    }

    function validateData(){
        if(origUser==="-1" || destUser ==="-1"){
            failedNotification(t('directrequest.noreqselected'))
            return false;
        }

        if(state.nlimit===""|| state.nlimit< 1 || state.nlimit > 100){
            failedNotification(t('safest.errstrength'))
            return false
        }
        return true
    }

    const obtainFLigPath = (event) => {
        event.preventDefault()
        
        if(!validateData()) return 

        const handler = state.emotion ? handlerFLigEmotion : handlerFLig

        handler(origUser, destUser, state.nlimit)
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

    const obtainFRelPath = (event) => {
        event.preventDefault()
        
        if(!validateData()) return 

        const handler = state.emotion ? handlerFRelEmotion : handlerFRel

        handler(origUser, destUser, state.nlimit)
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

    return (
        <Popup trigger={<button>{btn}</button>} modal
                nested> 
            {close=>(
                <div className="modal">
                    <button className="close" onClick={close}>
                        &times;
                    </button>
                    <div className="header">{header}</div>
                    
                    <div className="form__group field">
                        <input
                            type="number"
                            min={1}
                            max={100}
                            placeholder={t('nlimit.limit')}
                            onChange={handleChangeN}
                            name="nLimit"
                            id="nLimit"
                            value={state.nlimit}
                            required
                            className="form__field"
                        />
                        <label htmlFor="nLimit" className="form__label">{t('nlimit.limit')}</label>
                    </div>

                    <div className="emotion-checkbox">
                        <label>
                            <input type="checkbox" checked={state.emotion} onChange={handleCheckbox} />
                            {t('nlimit.emotion')}
                        </label>
                    </div>
                    
                    <div className="buttons-container">
                        <button onClick={obtainFLigPath}>{t('nlimit.flig')}</button>
                        <button onClick={obtainFRelPath}>{t('nlimit.frel')}</button>
                    </div>
                    
                    <TrajectViewer {...state} />
                </div>
            )}
        </Popup>
    )
}