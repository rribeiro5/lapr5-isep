import React, {useState, useEffect, useContext} from 'react';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import {toast} from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { createDirectRequest } from "../../services/ConnectionRequestService";
import { getUsersByName, getUserByEmail, getUsersByCountry, getUsersByTags } from "../../services/UserService";
import TagsInput from 'react-tagsinput';
import 'react-tagsinput/react-tagsinput.css'
import './DirectRequest.css'
import ProfilePreview from "../ProfilePreview/ProfilePreview";
import DirectConnectionRequest from "../DirectConnectionRequest/DirectConnectionRequest";
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';
import DirectRequestResult from "../DirectRequestResult/DirectRequestResult"

//TODO: Review, verificar gets e post

const TAGS = "Tags"
const COUNTRY = "Country"
const NAME = "Name"
const EMAIL = "Email"

export default function DirectRequest(props) {
    const {loggedUser} = useContext(Context)
    const currentUserId = loggedUser.id

    const { t } = useTranslation()

    const [state, setState] = useState({
        selectedCriteria: NAME,
        searchQuerie: "",
        users: []
    })
    
    useEffect(()=>{
        props.nameCurrentComponent(t('dirrequest.title'))
    },[])
    

    function handleChange(event) {
        const { name, value } = event.target
        setState(prevData => {
            return {
                ...prevData,
                [name]: value
            }
        }
        )
    }
    
    function handleInsertedValue(event){
        event.preventDefault()
        if(state.selectedCriteria === undefined){
            failedNotification(t('dirrequest.invcriteria'))
            return
        }

        if(state.searchQuerie === ""){
            failedNotification(t('dirrequest.emptyparam'))
            return;
        }
        
        
        switch (state.selectedCriteria) {
            case COUNTRY:
                getUsersByCountry(state.searchQuerie)
                    .then(res => handleUsers(res.data))
                    .catch(err => failedNotification(err.response.data));
                break;
            case NAME:
                getUsersByName(state.searchQuerie)
                    .then(res => handleUsers(res.data))
                    .catch(err => failedNotification(err.response.data));
                break;
            case EMAIL:
                getUserByEmail(state.searchQuerie)
                    .then(res => handleEmail(res.data))
                    .catch(err => failedNotification(err.response.data));
                break;
        }

    }
    
    function handleEmail(response){
        
        let component = (
            <div key={response.id} className="sugestion">
                <div className="user-info">
                    <ProfilePreview  user={response} />
                    <h3>{response.name}</h3>
                </div>
                <DirectConnectionRequest orig={currentUserId} dest={response} />
            </div>)
        setState({ ...state, users: [component]})
    }
    
    
    function handleUsers(users){
        if(users!==undefined){
            let components = []
            for(let index in users){
                
                let obj = users[index]
                if(obj.id===currentUserId) continue
                components.push(
                    <DirectRequestResult key={obj.id} user={obj}/>)
            }
            setState({ ...state, users: components })
        }
    }
    
    

    return (
        <div className="DirectRequestContainer">
            <form className="criteria-form IndirectConnectionForm">
                <select className="selector" value={state.selectedCriteria} name="selectedCriteria" onChange={handleChange}>
                    <option value="Name">{t('dirrequest.name')}</option>
                    <option value="Country">{t('dirrequest.country')}</option>
                    <option value="Email">{t('dirrequest.email')}</option>
                </select>
                <input type="text" className="criteria-input" name="searchQuerie" value={state.searchQuerie} onChange={handleChange}/>
                <button className="search" value="Search" onClick={handleInsertedValue}>{t('dirrequest.search')}</button>
            </form>
            
            <div className="Results">
                {state.users.length!==0 && state.users}
            </div>
            
            
        </div>

    )

}