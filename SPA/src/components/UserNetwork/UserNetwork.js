import React, {useState, useEffect, useContext} from "react"
import { toast } from "react-toastify"

import { getUserNetwork } from "../../services/UserService"

import './UserNetwork.css'
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from "react-i18next";

export default props => {
    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()
    
    const [state, setState] = useState({
        network: undefined,
        level: 1,
    })
    
    useEffect(()=>{
        props.nameCurrentComponent(t('usrntw.title'))
    },[])

    const networkList = (user) => {
        const hasConns = user.connections.length !== 0
        return (
            <li><span>{`${t('usrntw.level')} ${user.userLevel} - Email: ${user.email}`}</span>
                {hasConns && <ul>{user.connections.map((value) => networkList(value.user))}</ul>}
            </li>
        )
    }

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
        getUserNetwork(loggedUser.id, state.level)
            .then(res => setState({ ...state, network: res.data }))
            .catch((err) => failedNotification(err.response.data))
    }

    return (
        <div className="UserNetwork">
            <form className="modal" onSubmit={handleSubmit}>
                
                <div className="form__group field">
                    <input
                        type="number"
                        min={1} max={10}
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

                <input type="submit" value={t('usrntw.submit')} />
                
            </form>
            {state.network !== undefined && <ul>{networkList(state.network)}</ul>}
        </div>
    )
}