import React, {useContext} from 'react';
import Popup from 'reactjs-popup';
import Avatar from 'react-avatar';
import 'reactjs-popup/dist/index.css';
import {MdOutlineLocationOn,MdOutlineEmojiEmotions} from 'react-icons/md'
import {RiFacebookLine,RiLinkedinBoxLine} from 'react-icons/ri'
import {
    deleteUser,
    getPrivateProfile,
    getPrivateProfileByEmail,
    updatePrivateProfile,
    updateUserEmotionalState
} from '../../services/UserService';

import 'react-toastify/dist/ReactToastify.css';
import {CountryDropdown, RegionDropdown} from "react-country-region-selector";
import TagsInput from "react-tagsinput";
import {Context} from "../../context/loggedUser";
import {failedNotification, successNotification} from "../../utils/ToastContainerUtils";
import PopupContainer from "../PopupContainer/PopupContainer";
import { useTranslation } from 'react-i18next';
import NetworkStrength from "../NetworkStrength/NetworkStrength";

import './Profile.css'
import { useParams } from 'react-router-dom';

import Post from "../Post/Post";
import FeedPosts from '../FeedPosts/FeedPosts';
import {func} from "prop-types";

export default function Profile(props){

    const {loggedUser} = useContext(Context)

    const { email } = useParams()
    
    const { t } = useTranslation()
    
    const {setLoggedUser} = useContext(Context)

    const [profile, setProfile] = React.useState({
        "id":"",
        "avatar": "",
        "name": "",
        "email": "",
        "phoneNumber": "",
        "birthdayDate": "",
        "city": "",
        "country": "",
        "description": "",
        "points":"",
        "linkedInURL": "",
        "facebookURL": "",
        "interestTags": [],
        "emotionalState": "",
        isLoggedUser: false,
    })
    
    React.useEffect(()=>{
        props.nameCurrentComponent(t('profile.title'))
        
        loadProfile()
    },[])
    
    React.useEffect(()=>{
        loadProfile()
    },[email])
    
    function loadProfile() {
        if (email === undefined) {
            getPrivateProfile(loggedUser.id)
                .then(res => setProfile({ ...res.data, isLoggedUser: true }))
                .catch(err => failedNotification(err.response))
        } else {
            getPrivateProfileByEmail(email)
                .then(res => setProfile({ ...res.data, isLoggedUser: res.data.email == loggedUser.email }))
                .catch(err => failedNotification(err.response))
        }
    }

    function updateProfile(event){
        const {name,value} = event.target
        setProfile(prevData=>({...prevData,[name] : value}))
    }
    
    function deleteProfile(event){
        event.preventDefault()
        
        
        deleteUser(loggedUser.id)
            .then(()=>{
                setLoggedUser(undefined)
            })
            .catch(e => failedNotification(e.response) )
        
    }

    function handleSubmit(event){
        event.preventDefault()
        const body = {
            id: profile.id,
            state: profile.emotionalState
        }
        updateUserEmotionalState(profile.id, body)
        .then()
            .catch(err => failedNotification(err.response))
        
        if(profile.name.trim().length===0){
            failedNotification(t('profile.emptyName'))
            return 
        }
        
        const birthday=new Date(profile.birthdayDate)
        const current=new Date()
        const yearDiff=current.getFullYear()-birthday.getFullYear()
        if((yearDiff<16) 
        || (yearDiff===16 && birthday.getMonth() > current.getMonth())
        || (yearDiff===16 && birthday.getMonth() === current.getMonth() && birthday.getDate()>current.getDate())){
            failedNotification(t('profile.invalidDate'))
            return
        }

        updatePrivateProfile(profile.id,profile)
            .then( ()=> successNotification(t('profile.success')))
            .catch(err=> failedNotification(err.response.data.message))
    }
    
    
    return (
        <div className="profile-container">
            <div className="profile-info-container">
                <div className="profile-row-container">
                    <div className="userInfoContainer">
                        <Avatar size="90" name={profile.name} email={profile.email} round={true} src={profile.avatar} />
                        {/* <img className="round" src={profile.avatar} alt="profile"  /> */}
                        <h3>{profile.name}</h3>
                        <p>{profile.description}</p>
                        <h6> <MdOutlineLocationOn/> {profile.city}, {profile.country}</h6>
                        <h6> <RiFacebookLine/> {profile.facebookURL}</h6>
                        <h6> <RiLinkedinBoxLine/> {profile.linkedInURL}</h6>
                    </div>
                    <div className="profile-actions-container">
                        <NetworkStrength userId={profile.id} />
                        {profile.isLoggedUser && <Popup trigger={<button type="button">{t('profile.edittitle')}</button>} modal
                               nested>
                            {close=>(
                                <div className="modal">
                                    <button className="close" onClick={close}>
                                        &times;
                                    </button>
                                    <div className="header">{t('profile.edittitle')}</div>

                                    <div className="form__group field">
                                        <input
                                            type="text"
                                            value={profile.name}
                                            placeholder="Name"
                                            name="name"
                                            onChange={updateProfile}
                                            id="name"
                                            className="form__field"
                                        />
                                        <label htmlFor="name" className="form__label">{t('profile.name')}</label>
                                    </div>

                                    <div className="form__group field">
                                    <textarea
                                        value={profile.description}
                                        placeholder="Profile Description"
                                        onChange={updateProfile}
                                        name="description"
                                        className="form__field"
                                        id="description"
                                    />
                                        <label htmlFor="description" className="form__label">{t('profile.desc')}</label>
                                    </div>

                                    <div className="form__group field">
                                        <input
                                            type="text"
                                            value={profile.avatar}
                                            placeholder="Avatar"
                                            name="avatar"
                                            onChange={updateProfile}
                                            id="avatar"
                                            className="form__field"
                                        />
                                        <label htmlFor="avatar" className="form__label">{t('profile.avatar')}</label>
                                    </div>

                                    <div className="form__group field">
                                        <input
                                            type="text"
                                            value={profile.phoneNumber}
                                            placeholder="Phone Number"
                                            name="phoneNumber"
                                            onChange={updateProfile}
                                            id="phoneNumber"
                                            className="form__field"
                                        />
                                        <label htmlFor="phoneNumber" className="form__label">{t('profile.phone')}</label>
                                    </div>


                                    <div className="form__group field">
                                        <input
                                            type="date"
                                            onChange={updateProfile}
                                            name="birthdayDate"
                                            value={profile.birthdayDate}
                                            id="birthdayDate"
                                            className="form__field"
                                        />
                                    </div>

                                    <div className="form__group field">
                                        <CountryDropdown
                                            value={profile.country}
                                            name="country"
                                            className="form__field"
                                            onChange={(country) => setProfile({ ...profile, country })}
                                        />
                                    </div>

                                    <div className="form__group field">
                                        <RegionDropdown
                                            country={profile.country}
                                            name="city"
                                            value={profile.city}
                                            className="form__field"
                                            onChange={(city) => setProfile({ ...profile, city })}
                                        />
                                    </div>

                                    <div className="form__group field">
                                        <input
                                            type="text"
                                            placeholder="Linkedin"
                                            onChange={updateProfile}
                                            name="linkedInURL"
                                            className="form__field"
                                            value={profile.linkedInURL}
                                            id="linkedin"
                                        />
                                        <label htmlFor="linkedin" className="form__label">Linkedin</label>
                                    </div>

                                    <div className="form__group field">
                                        <input
                                            type="text"
                                            placeholder="Facebook"
                                            onChange={updateProfile}
                                            className="form__field"
                                            name="facebookURL"
                                            value={profile.facebookURL}
                                            id="facebook"
                                        />
                                        <label htmlFor="facebook" className="form__label">Facebook</label>
                                    </div>

                                    <div className="form__group field">
                                        <input
                                            type="text"
                                            value={profile.emotionalState}
                                            placeholder="Emotional State"
                                            name="emotionalState"
                                            onChange={updateProfile}
                                            id="emotionalState"
                                            className="form__field"
                                        />
                                        <label htmlFor="emotionalState" className="form__label">{t('profile.emotional')}</label>
                                    </div>

                                    <TagsInput
                                        value={profile.interestTags}
                                        addKeys={[9,13,32]}
                                        onChange={(interestTags)=>setProfile(prevData => {return {...prevData,interestTags}})}
                                        required />
                                    
                                    
                                    <Popup trigger={ <button className="deleteProfileButton" >{t('profile.deleteProfileButton')}</button>} modal
                                           nested>
                                        {close=>(
                                            <div className="modal">
                                                <button className="close" onClick={close}>
                                                    &times;
                                                </button>
                                                
                                                <div className="header">{t('profile.deleteProfileButton')}</div>

                                                <div className="delete-profile-container">
                                                    <p>{t('profile.deleteText')}</p>
                                                </div>

                                                <button className="deleteProfileButton" onClick={deleteProfile}>{t('profile.deleteProfileButton')}</button>
                                            </div>
                                        )}
                                    </Popup>
                                    <button className="submitButton" onClick={handleSubmit}>{t('profile.save')}</button>
                                </div>
                            )}
                        </Popup>}
                    </div>
                </div>
            </div>
            
            <div className="profile-feed-container">
                {profile.isLoggedUser && <Post userId={profile.id} />}
                <FeedPosts user={profile} />
            </div>
            
        </div>
    )
}