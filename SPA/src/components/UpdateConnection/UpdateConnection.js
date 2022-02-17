import React, {useState, useEffect} from "react"
import TagsInput from 'react-tagsinput'

import Connection from "../../model/Connection/Connection"
import UpdateConnectionDTO from "../../model/Connection/UpdateConnectionDTO"
import { getConnectionsOfUser, updateConnection } from "../../services/ConnectionService"
 
import 'react-tagsinput/react-tagsinput.css'
import './UpdateConnection.css'
import {failedNotification, successNotification} from "../../utils/ToastContainerUtils";

import { useTranslation } from "react-i18next";

export default props => {
    
    const { t } = useTranslation()
    
    const [state, setState] = useState({
        connStrength: props.connection.connectionStrength,
        tags: props.connection.tags,
    })

    const handleConnStrength = (event) => {
        setState({ ...state, connStrength: event.target.value })
    }

    const handleSubmit = (event) => {
        event.preventDefault()

        if (1 > state.connStrength > 100) {
            failedNotification(t('editconnections.invalidconnstrength'))
            return
        }
        const data = new UpdateConnectionDTO(state.connStrength, state.tags)
        updateConnection(props.connection.id, data)
            .then(() => {
                successNotification(t('editconnections.success'))
            })
            .catch(err => {
                failedNotification(err.response.date)
            })
    }
    
    

    return (
        <div className="updateConnection">
            <form className="updateConnectionForm" onSubmit={handleSubmit}>
                <div className="form__group field">
                    <input type="number" name="connStrength" placeholder="Connection Strength" required
                           min={1} max={100} value={state.connStrength} onChange={handleConnStrength} className="form__field" />
                    <label htmlFor="connStrength" className="form__label">{t('editconnections.cstrengthlabel')}</label>
                </div>

                <TagsInput value={state.tags} onChange={(tags) => setState(prevData => { return { ...prevData, tags }})} addKeys={[9,13,32]} />
                
                <input type="submit" value={t('formdefaults.submit')} />
            </form>
        </div>
        
    )
}