import Popup from "reactjs-popup";
import React, {useContext, useEffect, useState} from "react";
import {useTranslation} from "react-i18next";
import {getMutualFriends} from "../../services/UserService"

import {failedNotification} from "../../utils/ToastContainerUtils";
import {Context} from "../../context/loggedUser";
import ProfilePreview from "../ProfilePreview/ProfilePreview";
import "./FriendsInCommon.css"
export default function FriendsInCommon({destUserId}){
    
    const {loggedUser} = useContext(Context)
    const { t } = useTranslation()
    
    const[state,setState] = useState({
        friends:[]
    })
    
    useEffect(()=>{
        getMutualFriends(loggedUser.id,destUserId)
            .then(res=>convertToProfilePreview(res.data))
            .catch(()=>failedNotification("Unexpected error"))
    },[])
    
    function convertToProfilePreview(data){
        const result = data.map(
            user => {
             return(   
                 <div key={user.id} className="common-friend">
                     <ProfilePreview key={user.id} user={user}/>
                     <h3>{user.name}</h3>
                 </div>
             )
            }
        )
        setState(() => {
            return {
                friends: result 
            }
        })
    }
    
    
    return(
        <Popup trigger={<button>{state.friends.length}{t('friendsInCommon.button')}</button>} modal
               nested>
            {close => (
                <div className="modal">
                    <button className="close" onClick={close}>
                        &times;
                    </button>
                    <div className="header">{t('friendsInCommon.button')}</div>
                    {
                        state.friends.length>0?
                            <div className="common-friends-main-container">
                                <div className="common-friends-text">
                                    <h3>
                                        <span className="common-friends-number">{state.friends.length}</span> {t('friendsInCommon.text')}
                                    </h3>
                                </div>
                                <div className="common-friends">
                                    {state.friends}
                                </div>
                            </div>
                            :
                            <div className="common-friends-text">
                                <h3>
                                    No friends in common !
                                </h3>
                            </div>
                    }
                    
                </div>
            )}
        </Popup>
    )
}