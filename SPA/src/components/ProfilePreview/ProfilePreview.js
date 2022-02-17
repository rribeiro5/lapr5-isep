import React, {useContext, useEffect, useState} from 'react';
import Avatar from 'react-avatar';
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import './ProfilePreview.css'
import {MdOutlineLocationOn} from "react-icons/md";
import {RiFacebookLine, RiLinkedinBoxLine} from "react-icons/ri";

import NetworkStrength from "../NetworkStrength/NetworkStrength";
import NetworkSize from "../NetworkSize/NetworkSize";
import {useTranslation} from "react-i18next";
import {Context} from "../../context/loggedUser";
import {getConnectionsOfUser} from "../../services/ConnectionService";
import { Link } from 'react-router-dom';


export default function ProfilePreview({user}){
    
    const [friend,setFriendState] = useState(false)

    const { t } = useTranslation()
    const {loggedUser} = useContext(Context)

    useEffect(()=>{
        getConnectionsOfUser(loggedUser.id)
            .then(res =>{

                res.data.connections.forEach(connection =>{
                    if(connection.dUser.value === user.id || connection.oUser.value === user.id){
                        setFriendState(true)
                    }
                })
            })
    },[])
    
    
    return (


        <Popup trigger={<div className="ProfilePreview">
                            <Avatar size="60" name={user.name} email={user.email} round={true} src={user.avatar} />
                        </div>
            
        } modal nested>
            {close=>(
                <div className="modal">
                    <button className="close" onClick={close}>
                        &times;
                    </button>
                    <div className="header"> {user.name} </div>
                    <div className="profile-preview-container">
                        <div className="profilePreview-row-container">
                            <div className="userInfoContainer">
                                <Avatar size="65" name={user.name} email={user.email} round={true} src={user.avatar} />
                                <h3>{user.name}</h3>
                                {user.city&&<p>{user.description}</p>}
                                {user.city && <h6> <MdOutlineLocationOn/> {user.city}, {user.country}</h6> }
                                {user.facebookURL && <h6> <RiFacebookLine/> {user.facebookURL}</h6>}
                                {user.linkedInURL && <h6> <RiLinkedinBoxLine/> {user.linkedInURL}</h6>}
                            </div>
                            <div className="profilePreview-actions">
                                {friend &&<span className="friend-span">{t("directRequestResult.friend")}</span>}
                                <Link className="view-profile-action" to={`/profile/${user.email}`}>{t('profilePreview.viewprofile')}</Link>
                                <NetworkStrength userId={user.id} />
                                <NetworkSize  userId={user.id} />
                            </div>
                        </div>   
                    </div>
                </div>    
            )}
        </Popup>
        
        
        
        
    )
}