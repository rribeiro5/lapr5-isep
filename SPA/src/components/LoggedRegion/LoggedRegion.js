
import 'react-toastify/dist/ReactToastify.css';

import './LoggedRegion.css'
import LoggedSidebar from '../LoggedSidebar/LoggedSidebar';
import MainContainer from '../MainContainer/MainContainer';
import React, {useContext, useEffect} from "react";
import SecondaryContainer from "../SecondaryContainer/SecondaryContainer";
import {Context} from "../../context/loggedUser";
import {successNotification} from "../../utils/ToastContainerUtils";
import {ToastContainer} from "react-toastify";
import { useTranslation } from 'react-i18next';

export default function LoggedRegion(){

    const {loggedUser} = useContext(Context)

    const { t } = useTranslation()
    
    useEffect(()=>{
        successNotification(`${t('loggedregion.welcome')} ${loggedUser.name}!`)
    }, [])
    
    return (
        <div className="LoggedRegionContainer">
            <LoggedSidebar/>
            <MainContainer/>
            <SecondaryContainer/>
            <ToastContainer
                position="top-center"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
            />
        </div>
        
    )
}