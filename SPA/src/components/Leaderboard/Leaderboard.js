import React, {useEffect, useState} from "react";
import {useTranslation} from "react-i18next";
import Avatar from 'react-avatar';
import {leaderboardNetworkDimension, leaderboardNetworkStronghold} from "../../services/LeaderboardService"
import {failedNotification} from "../../utils/ToastContainerUtils";

import {BiTrophy,BiMedal} from 'react-icons/bi'

import "./Leaderboard.css"
import {FaRegBell} from "react-icons/fa";

export default function Leaderboard(props){

    const { t } = useTranslation()
    const [state,setState] = useState({
        dimension:[],
        stronghold:[]
    })
    
    // buscar a dimensao da rede e fortaleza
    useEffect(()=>{
        props.nameCurrentComponent(t('leaderboards.title'))
        leaderboardNetworkDimension()
            .then(res => getTop3UsersDimension(res.data))
            .catch(() => failedNotification("Error on network"))
        leaderboardNetworkStronghold()
            .then(res => getTop3UsersStronghold(res.data))
            .catch(() => failedNotification("Error on network"))
    },[])

    

    function getTop3UsersDimension(result){
        const converted = []
        for(let index=0;index<3;index++){
            let data = result[index]
            converted[index] =
                <li className="leaderboard-user" key={data.id}>
                    
                    <Avatar size="50" name={data.name} email={data.email} round={true} src={data.avatar} />
                    <div className="leaderboard-user-info-container">
                        <h3>{data.name}</h3>
                        <h3>{data.value} {t('leaderboards.points')}</h3>
                    </div>
                    {index===0?<BiTrophy color="red"/>:<BiMedal />}
                </li>
        }
        
        setState(prevState => {
            return {
                ...prevState,
                dimension: converted
            }
        })
        
    }
    
    function getTop3UsersStronghold(result){
        const converted = []
        for(let index=0;index<3;index++){
            let data = result[index]
            converted[index] =
                <li className="leaderboard-user" key={data.id}>
                    
                    <Avatar size="50" name={data.name} email={data.email} round={true} src={data.avatar} />
                    <div className="leaderboard-user-info-container">
                        <h3>{data.name}</h3>
                        <h3>{data.value} {t('leaderboards.points')}</h3>
                    </div>
                    {index===0?<BiTrophy color="red"/>:<BiMedal />}
                </li>
        }
        
        setState(prevState => {
            return {
                ...prevState,
                stronghold: converted
            }
        })
        
    }
    
    return(
        <section className="leaderboard-main-container">
            <div className="leaderboard-secondary-container">
                <div className="leaderboard-secondary-container-title">
                    <h3>{t('leaderboards.dimension')}</h3>
                </div>
                <ul className="leaderboard">
                    {state.dimension}
                </ul>
            </div>
            
            <div className="leaderboard-secondary-container">
                <div className="leaderboard-secondary-container-title">
                    <h3>{t('leaderboards.stronghold')}</h3>
                </div>
                <ul>
                    {state.stronghold}
                </ul>
            </div>
            
        </section>
    )
}