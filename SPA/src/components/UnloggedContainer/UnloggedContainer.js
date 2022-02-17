import Register from '../Register/Register';
import Login from '../Login/Login';

import PopupContainer from "../PopupContainer/PopupContainer";
import {ToastContainer} from "react-toastify";
import React from "react";
import { useTranslation } from 'react-i18next';
import Ilustration from "../../assets/images/loginIlustration.svg"
import './UnloggedContainer.css'

export default function UnloggedContainer(){
        const { t } = useTranslation()
    
    
        return (
            <div className='unlogged-background'>
                <div className="unlogged-container">
                    <div className="unlogged-image">
                        <div className="unlogged-image-container">
                            <img src={Ilustration} alt="image" />
                        </div>
                    </div>
                    <div className="unlogged-content">
                        <div className="unlogged-content-container">
                            <div className="unlogged-content-text">
                                <h3 className="unlogged-content-sign" >{t('unlogged.sloggan')}</h3>
                                <p className="unlogged-content-login">{t('unlogged.login')}</p>
                            </div>
                            <Login   />
                            <p>{t('unlogged.register')}</p>
                            <PopupContainer Component={<Register   />}  title={t('unlogged.btnregister')} />
                        </div>
                    </div>
                </div>
                
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

