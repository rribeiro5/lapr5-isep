
import React,{useState,useContext} from 'react';
import 'react-toastify/dist/ReactToastify.css';


import {login} from "../../services/UserService";
import LoginDto from "../../model/User/LoginDto";

import {Context} from "../../context/loggedUser";
import {failedNotification} from "../../utils/ToastContainerUtils";
import { useTranslation } from 'react-i18next';
import { css } from "@emotion/react";
import PuffLoader from "react-spinners/PuffLoader";

import './Login.css'

export default function Login(){
    
 let [loading, setLoading] = useState(false);
 const {setLoggedUser} = useContext(Context)

 const { t } = useTranslation()
    
 const [input,setInputData] = useState(
     {
            Email:"",
            Password:""
         }
    )

    function handleChange(event){
        const {name,value} = event.target
        setInputData(prevData=>{
                return {
                    ...prevData,
                    [name]:value
                }
            }
        )
    }
    
    const handleSubmit = (event) =>{
        setLoading(true)
        event.preventDefault()
        const {Email,Password} = input
        let userLogin =new LoginDto (
            Email,
            Password
        )
        
        login(userLogin)
            .then( userInfo => {
                setLoggedUser(userInfo.data)
                }
            )
            .catch( () => failedNotification(t('login.fail')))
            .finally(()=>setLoading(false))
    }
    
    return(
        <div className="login-main-container">
            <form onSubmit={handleSubmit} className="LoginFormContainer">

                <div className="form__group field">
                    <input
                        type="text"
                        placeholder="Email"
                        onChange={handleChange}
                        name="Email"
                        id="Email"
                        value={input.email}
                        required
                        className="form__field"
                    />
                    <label htmlFor="email" className="form__label">Email</label>
                </div>

                <div className="form__group field">
                    <input
                        type="password"
                        placeholder="Password"
                        onChange={handleChange}
                        name="Password"
                        id="password"
                        value={input.Password}
                        required
                        className="form__field"
                    />
                    <label htmlFor="password" className="form__label">Password</label>
                </div>
                
                <button type="submit"  >
                    Login
                    <PuffLoader color="#fff" loading={loading}  size={20} />
                </button>
                
                
            </form>
        </div>
        )
    
}    