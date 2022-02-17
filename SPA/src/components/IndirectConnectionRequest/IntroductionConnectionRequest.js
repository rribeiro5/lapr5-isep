import React, {useState, useEffect, useContext} from "react"

import {getPossibleDestinyUsers} from "../../services/ConnectionService";
import {getMutualFriends} from "../../services/UserService";
import ConnectionRequestUserDTO from "../../model/User/ConnectionRequestUserDTO";
import TagsInput from 'react-tagsinput'
import 'react-tagsinput/react-tagsinput.css'
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import IntroductionRequestDTO from "../../model/ConnectionRequest/IntroductionRequestDTO";
import {introductionRequest} from "../../services/ConnectionRequestService";
import './IndirectConnectionRequest.css'
import {Context} from "../../context/loggedUser";
import {failedNotification, successNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from "react-i18next";


export default function IntroductionConnectionRequest(props){

    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()

    const defaultOption = <option key="-1" value="-1" >{t('introductionreq.selectdefault')}</option>
    const currentUserId = loggedUser.id
    
    
    const[state,setState] = useState({
        possibleDestinyUsers: [defaultOption],
        selectedDestinyUser: "-1",
        possibleIntermediaries: [defaultOption],
        selectedIntermediarie: "-1",
        messageOrigToDest: "",
        messageOrigToInter: "",
        connectionStrength: 0,
        Tags:[]
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
    
    function convertUsersToOptions(data){
        let usersDto = data.map(u => new ConnectionRequestUserDTO(u.id,u.email,u.name))
        return usersDto.map(dto=><option key={dto.UserId} value={dto.UserId}>{dto.Name}</option>)
    }
    
    
  
    
    useEffect(()=>{
            props.nameCurrentComponent(t('introductionreq.title'))
            getPossibleDestinyUsers(currentUserId)
                .then(res => setState(prevState=>{
                    const componentUsersDto = convertUsersToOptions(res.data)
                    return{
                        ...prevState,
                        possibleDestinyUsers: [defaultOption,componentUsersDto]
                    }
                }))
                .catch(e=>failedNotification(e.response.data))
        }
    ,[])


    
    
    
    // Use effect que fica atento a mudanca do destiny user
    
    useEffect(()=>{
            if(state.selectedDestinyUser !== undefined){
                getMutualFriends(currentUserId,state.selectedDestinyUser)
                    .then(res => setState( prevState => {
                        const componentUsersDto = convertUsersToOptions(res.data)
                        return{
                            ...prevState,
                            possibleIntermediaries: [defaultOption,componentUsersDto]
                        }
                    }))
                    .catch(e=>failedNotification(e.response.data))
            }
        }
    ,[state.selectedDestinyUser])

    const handleSubmit = (event) =>{
        event.preventDefault();
        
        if(state.selectedDestinyUser === "-1"){
            failedNotification(t('introductionreq.failnodest'))
            return
        }

        if(state.selectedIntermediarie === "-1"){
            failedNotification(t('introductionreq.failnointer'))
            return
        }

        if(state.messageOrigToDest === ""){
            failedNotification(t('introductionreq.failmsgdest'))
            return
        }

        if(state.messageOrigToInter === ""){
            failedNotification(t('introductionreq.failmsginter'))
            return
        }

        if(state.Tags.length === 0){
            failedNotification(t('directrequest.failnotags'))
            return
        }
 
        
        const{selectedDestinyUser,selectedIntermediarie,messageOrigToDest,messageOrigToInter,connectionStrength,Tags} = state
        const data = new IntroductionRequestDTO(currentUserId,selectedIntermediarie,selectedDestinyUser,messageOrigToDest,messageOrigToInter,connectionStrength,Tags);
        introductionRequest(data)
            .then(()=>{
                setState(() => {
                    return {
                        possibleDestinyUsers: [defaultOption],
                        selectedDestinyUser: "-1",
                        possibleIntermediaries: [defaultOption],
                        selectedIntermediarie: "-1",
                        messageOrigToDest: "",
                        messageOrigToInter: "",
                        connectionStrength: 0,
                        Tags:[]
                    }
                })
                successNotification(t('introductionreq.success'))
            })
            .catch(err=>failedNotification(err.response.data))
    }
    
    
    return(
        <div className="IndirectConnectionsContainer">
            <form onSubmit={handleSubmit} className="IndirectConnectionForm" >
                
                <div className="selectContainer">
                    <label htmlFor="destinyUserSelect">{t('introductionreq.destuser')}</label>
                    <select 
                        id="destinyUserSelect"
                        value={state.selectedDestinyUser}
                        onChange={handleChange}
                        name="selectedDestinyUser"
                    >
                        {state.possibleDestinyUsers}
                    </select>
                </div>

                <div className="selectContainer">
                    <label htmlFor="IntermediaryUserSelect">
                        {t('introductionreq.interuser')}</label>
                    <select
                        id="IntermediaryUserSelect"
                        value={state.selectedIntermediarie}
                        onChange={handleChange}
                        name="selectedIntermediarie"
                    >
                        {state.possibleIntermediaries}
                    </select>
                </div>

                <div className="form__group field">
                    <input
                        type="text"
                        placeholder="Message to Destiny User"
                        onChange={handleChange}
                        name="messageOrigToDest"
                        id="messageOrigToDest"
                        value={state.messageOrigToDest}
                        className="form__field"
                    />
                    <label htmlFor="messageOrigToDest" className="form__label">{t('directrequest.msgdest')}</label>
                </div>

                <div className="form__group field">
                    <input
                        type="text"
                        placeholder="Message to Intermediary User"
                        onChange={handleChange}
                        name="messageOrigToInter"
                        id="messageOrigToInter"
                        value={state.messageOrigToInter}
                        className="form__field"
                    />
                    <label htmlFor="messageOrigToInter" className="form__label">{t('introductionreq.msginter')}</label>
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
    )
}