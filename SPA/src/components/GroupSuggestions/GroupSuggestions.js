import React, {useState, useEffect, useContext} from 'react'
import   '@fortawesome/fontawesome-svg-core'
import '@fortawesome/free-brands-svg-icons'
import  '@fortawesome/free-solid-svg-icons'
import {useTranslation} from "react-i18next";
import DualListBox from 'react-dual-listbox';
import TagsInput from 'react-tagsinput'

import {getAllUsers, getPrivateProfile} from "../../services/UserService";
import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import GroupSuggestionDTO from "../../model/User/GroupSuggestionDTO";
import {getGroupSuggestions} from "../../services/UserNetworkService";

import DirectRequestResult from "../DirectRequestResult/DirectRequestResult";
import PuffLoader from "react-spinners/PuffLoader";

import 'react-dual-listbox/lib/react-dual-listbox.css';
import "./GroupSuggestions.css"

export default function GroupSuggestions({nameCurrentComponent}){

    let [loading, setLoading] = useState(false);

    const { t } = useTranslation()
    
    const [availableUsers,setAvailableUsers] = useState([])

    const [groupSuggestions,setGroupSuggestions] = useState([])

    const [usersToShow,setUsersToShow] = useState([])

    const {loggedUser} = useContext(Context)

    useEffect(()=>{
        nameCurrentComponent(t('sidebar.groupSuggestions'))
        getAllUsers()
            .then(res =>{
                let result = []
                res.data.forEach(user =>{
                    if(user.id !== loggedUser.id){
                        result.push({
                            value: user.id,
                            label: user.email
                        })
                    }
                })
                setAvailableUsers(result)})
            .catch(err => console.error(err))

    },[])


    
    const [input,setInput] = useState({
        lTagsObrigatorias:[],
        nTagsComum:0,
        nMinimoUsers:0,
        desired:[],
        toAvoid:[]
    })

    useEffect(()=>{
        setInput(prevState => {
            return {
                ...prevState,
                nTagsComum: input.lTagsObrigatorias.length
            }
        })
    },[input.lTagsObrigatorias.length])

    useEffect(()=>{
        const usersProfile = []
        groupSuggestions.forEach( userId => {
            getPrivateProfile( userId)
                .then(res =>{
                    usersProfile.push(res.data)
                    if (usersProfile.length === groupSuggestions.length) {
                        setUsersToShow(usersProfile)
                    }
                } )
                .catch(err => console.error(err))
        })
    },[groupSuggestions.length])

    function handleChange(event){
        const {name,value} = event.target
        setInput(prevData=>{
                return {
                    ...prevData,
                    [name]:value
                }
            }
        )
    }
    
    const updateDesiredUsers = (desired) => {
        setInput(prevState => {
            return{
                ...prevState,
                desired
            }
        })
    }
    
    const updateUsersAvoid = (toAvoid) =>{
        setInput(prevState => {
            return{
                ...prevState,
                toAvoid
            }
        })
    }
    
    function handleSubmit(event){
        event.preventDefault()
        setLoading(true)
        const {lTagsObrigatorias,
            nTagsComum,
            nMinimoUsers,
            desired,
            toAvoid} = input

        const groupSuggestionDTO =
            new GroupSuggestionDTO(loggedUser.id,lTagsObrigatorias,nTagsComum,nMinimoUsers,desired,toAvoid)

        getGroupSuggestions(groupSuggestionDTO)
            .then(res => setGroupSuggestions(res.data))
            .catch(err=> {
                console.error(err)
            })

        setLoading(false)

    }
    
    return(
        <div className="group-suggestion-main-container">
            <form onSubmit={handleSubmit} className="group-suggestion-form">
                <h3>{t('groupSuggestions.desiredTags')}</h3>
                <TagsInput value={input.lTagsObrigatorias} addKeys={[9,13,32]}  onChange={(lTagsObrigatorias)=>setInput(prevData => {return {...prevData,lTagsObrigatorias}})} required />


                <div className="form__group field">
                    <input
                        type="number"
                        min={input.lTagsObrigatorias.length}
                        max={input.lTagsObrigatorias.length+10}
                        placeholder={t('groupSuggestions.minimumDesiredtags')}
                        onChange={handleChange}
                        name="nTagsComum"
                        id="nTagsComum"
                        value={input.nTagsComum}
                        className="form__field"
                    />
                    <label htmlFor="nTagsComum" className="form__label">{t('groupSuggestions.minimumDesiredtags')}</label>
                </div>

                <div className="form__group field">
                    <input
                        type="number"
                        min="0"
                        max={availableUsers.length}
                        placeholder={t('groupSuggestions.minimumNumberOfUsers')}
                        onChange={handleChange}
                        name="nMinimoUsers"
                        id="nMinimoUsers"
                        value={input.nMinimoUsers}
                        className="form__field"
                    />
                    <label htmlFor="nMinimoUsers" className="form__label">{t('groupSuggestions.minimumNumberOfUsers')}</label>
                </div>
                
                
                <div className="group-suggestion-list-container">
                    <h3 className="group-suggestion-list-title">{t('groupSuggestions.desiredUsers')}</h3>
                    <DualListBox
                        options={availableUsers}
                        selected={input.desired}
                        onChange={updateDesiredUsers}
                    />
                </div>
                <div className="group-suggestion-list-container">
                    <h3 className="group-suggestion-list-title">{t('groupSuggestions.afraidUsers')}</h3>
                    <DualListBox
                        options={availableUsers}
                        selected={input.toAvoid}
                        onChange={updateUsersAvoid}
                    />
                </div>

                <button  className="submitButton" type="submit">
                    {t('groupSuggestions.buttonSuggestions')}
                    <PuffLoader color="#fff" loading={loading}  size={20} />
                </button>

            </form>

            {usersToShow.length>0 ?
                <div className="group-suggestions-viewer-container">
                    <div className="group-suggestions-viewer-title">
                        <h3>{t('groupSuggestions.result')}</h3>
                    </div>
                    <ul className="group-suggestions-viewer">

                        {usersToShow.map(user=>{
                            return (
                                <li className="group-suggestions-viewer-user" key={user.id}>
                                    <DirectRequestResult key={user.id} user={user}/>
                                </li>)
                        })}
                    </ul>
                </div>
            : false}
            
        </div>
    )
}