import React,{useState} from 'react';
import 'reactjs-popup/dist/index.css';
import 'react-tagsinput/react-tagsinput.css'
import { CountryDropdown, RegionDropdown } from 'react-country-region-selector';
import PasswordInput from '../PasswordInput/PasswordInput'
import TagsInput from 'react-tagsinput'
import {registerUser} from "../../services/UserService";
import CreatingUserDto from "../../model/User/CreatingUserDto";
import PopupContainer from "../PopupContainer/PopupContainer";
import {failedNotification, successNotification} from "../../utils/ToastContainerUtils";
import './Register.css'
import { useTranslation } from 'react-i18next';
import Popup from "reactjs-popup";
export default function Register(){
    
    const { t } = useTranslation()
    
    let[formData,setFormData] = useState(
        {
            email:"",
            telephoneNumber:"",
            name:"",
            avatar:"",
            city:"",
            country:"",
            linkLinkedin:"",
            linkFacebook:"",
            password:"",
            confirmPassword:"",
            birthdaydate:"",
            description:"",
            interestTags:[],
            consentRegister:false
        }
    )
    
    
    function handleChange(event){
        const {name,value} = event.target
        setFormData(prevData=>{
                return {
                    ...prevData,
                    [name]:value
                }
            }
        )
    }
    
    function handleCheckbox(event){
        setFormData(prevState => {
            return {
                ...prevState,
                consentRegister: event.target.checked
            }
        })
    }
    
    const handleSubmit = (event) =>{
      
        event.preventDefault()
        if(!validateData()) return
        const{email,telephoneNumber,name,avatar,city,country,linkFacebook,linkLinkedin,password,birthdaydate,description,interestTags} = formData
        
        const creatingUserDto = new CreatingUserDto(
            email,
            telephoneNumber,
            name,
            avatar,
            city,
            country,
            linkLinkedin,
            linkFacebook,
            password,
            birthdaydate,
            description,
            interestTags
        )
        
        registerUser(creatingUserDto)
            .then( () => {
                successNotification(t('register.success'))
            })
            .catch(err => failedNotification(err.response))
    }
    
    function validateData(){
        const{email,name,password,confirmPassword,birthdaydate,interestTags} = formData
        if(email===""){
            failedNotification(t('register.erremail'))
            return false
        }
        if(name===""){
            failedNotification(t('register.errname'))
            return false
        }
        if(birthdaydate===""){
            failedNotification(t('register.errbirthdate'))
            return false
        }
        if(password===""||confirmPassword==="") {
            failedNotification(t('register.errnopwd'))
            return false
        }
        if(password!==confirmPassword) {
            failedNotification(t('register.errconfimrpwd'))
            return false
        }
        if(interestTags.length===0){
            failedNotification(t('register.errnotags'))
            return false
        }
        
        if(!formData.consentRegister){
            failedNotification(t('register.consent'))
            return false
        }
        
        return true
    }
    
    function RGPD (){
        return (
            <Popup trigger={<button  type="button" className="rgpd-trigger">{t('gdpr.title')}</button>} modal
                   nested>
                {close=>(
                    <div className="modal">
                        <button className="close" onClick={close}>
                            &times;
                        </button>
                        <div className="header">{t('gdpr.title')}</div>
                        <div className="RGPD-main-container">
                            
                            <h2>{t('gdpr.topic1')}</h2>
                            <p>{t('gdpr.controller')}</p>
                            <br/>
                            <h2>{t('gdpr.topic2')}</h2>
                            <ol type="1" className="ordered-lst">
                                <li className="ordered-li">{t('gdpr.purpose1')}</li>
                                <br/>
                                    <ul>
                                        <li>{t('gdpr.data1')}</li>
                                        <li>Tags</li>
                                        <li>LinkedIn</li>
                                        <li>{t('gdpr.data2')}</li> 
                                        <li>{t('gdpr.data3')}</li>
                                    </ul>
                                    
                                    <h3>{t('gdpr.h3')}</h3>
                                    <p>{t('gdpr.legalBasis')}</p>
                                <br/>
                                <li className="ordered-li">{t('gdpr.purpose2')}</li>
                                <br/>
                                    <ul>
                                        <li>Email</li>
                                        <li>{t('gdpr.data4')}</li>   
                                    </ul>
        
                                    <h3>{t('gdpr.h3')}</h3>
                                    <p>{t('gdpr.legalBasis')}</p>
                                <br/>
                                <li className="ordered-li">{t('gdpr.purpose3')}</li>
                                <br/>
                                    <ul>
                                        <li>Facebook</li>
                                    </ul>
                                    <h3>{t('gdpr.h3')}</h3>
                                    <p>{t('gdpr.legalBasis')}</p>
                                <br/>
        
                                <li className="ordered-li">{t('gdpr.purpose4')}</li>
                                <br/>
                                    <ul>
                                        <li>Avatar</li>
                                    </ul>
                                    <h3>{t('gdpr.h3')}</h3>
                                    <p>{t('gdpr.legalBasis')}</p>
                                <br/>
        
                                <li className="ordered-li">{t('gdpr.purpose5')}</li>
                                <br/>
                                    <ul>
                                        <li>{t('gdpr.data5')}</li>
                                    </ul>
                                    <h3>{t('gdpr.h3')}</h3>
                                    <p>{t('gdpr.legalBasis')}</p>
                                <br/>
        
                                <li className="ordered-li">{t('gdpr.purpose6')}</li>
                                <br/>
                                    <ul>
                                        <li>{t('gdpr.data6')}</li>
                                    </ul>
                                    <h3>{t('gdpr.h3')}</h3>
                                    <p>{t('gdpr.legalBasis')}</p>
                                <br/>
        
                                <li className="ordered-li">{t('gdpr.purpose7')}</li>
                                <br/>
                                    <ul>
                                        <li>Post</li>
                                        <li>{t('gdpr.data7')}</li>
                                    </ul>
                                    <h3>{t('gdpr.h3')}</h3>
                                <p>{t('gdpr.legalBasis')}</p>
                                <br/>
                            </ol>


                            <h2>{t('gdpr.topic3')}</h2>
                            <ul>
                                <li>{t('gdpr.right1')}</li>
                                <li>{t('gdpr.right2')}</li>
                                <li>{t('gdpr.right3')}</li>
                                <li>{t('gdpr.right4')}</li>
                                <li>{t('gdpr.right5')}</li>
                                <li>{t('gdpr.right6')}</li>

                            </ul>
                        </div>
                    </div>
                )}
            </Popup>
        )
    }
    

    return(
        <div className='FormContainer'>
            <form  className="RegisterFormContainer">


                <div className="form__group field">
                    <input 
                        type="text"
                        placeholder="Email"
                        onChange={handleChange}
                        name="email"
                        id="Email"
                        value={formData.email}
                        className="form__field"
                    />
                    <label htmlFor="email" className="form__label">{t('profile.email')}</label>
                </div>

                <div className="form__group field">
                        <input
                            type="text"
                            placeholder="Name"
                            onChange={handleChange}
                            name="name"
                            id="name"
                            value={formData.name}
                            className="form__field"
                        />
                    <label htmlFor="name" className="form__label">{t('profile.name')}</label>
                </div>    
                    
                <div className="form__group field">
                    <input
                        type="date"
                        onChange={handleChange}
                        name="birthdaydate"
                        value={formData.birthdaydate}
                        id="birthdaydate"
                        className="form__field"
                    />
                </div>

                
                <div className="form__group field PasswordForm">
                    <PasswordInput 
                        placeholder="Password"
                        value={formData.password}
                        name="password"
                        onChange={setFormData}
                        id="password"
                        className="form__field"
                    />
                </div>
                
                
                <div className="form__group field PasswordForm">
                    <PasswordInput 
                        placeholder="Confirm Password"
                        value={formData.confirmPassword}
                        name="confirmPassword"
                        id="confirmPassword"
                        className="form__field"
                        onChange={setFormData} />
                   
                </div>

                <TagsInput value={formData.interestTags} addKeys={[9,13,32]}  onChange={(interestTags)=>setFormData(prevData => {return {...prevData,interestTags}})} required />

                {/* <PopupContainer Component={<ProfileInformation />} title={t('register.profileinfo')}/> */}
                <Popup trigger={<button className="popup-button" type="button"> {t('register.profileinfo')} </button>} modal
                    nested>
                    {close=>(
                        <div className="modal">
                            <button className="close" onClick={close}>
                                &times;
                            </button>
                            <div className="header"> {t('register.profileinfo')} </div>
                            <div className="profile-info">
                                <form onSubmit={handleSubmit} className="RegisterFormContainer">
                                        <div className="form__group field">
                                            <input
                                                type="text"
                                                placeholder="Avatar Url"
                                                onChange={handleChange}
                                                className="form__field"
                                                name="avatar"
                                                value={formData.avatar}
                                                id="avatar"
                                            />
                                            <label htmlFor="avatar" className="form__label">Avatar Url</label>
                                        </div>

                                        <div className="form__group field">
                                            <input
                                                type="tel"
                                                placeholder="Telephone Number"
                                                onChange={handleChange}
                                                name="telephoneNumber"
                                                value={formData.telephoneNumber}
                                                pattern="\+[0-9]{9,13}"
                                                className="form__field"
                                                id="telephone"
                                            />
                                            <label htmlFor="telephone" className="form__label">{t('profile.phone')}</label>
                                        </div>

                                        <div className="form__group field">
                                            <CountryDropdown
                                                value={formData.country}
                                                name="country"
                                                className="form__field"
                                                onChange={(country) => setFormData({ ...formData, country })}
                                            />
                                        </div>

                                        <div className="form__group field">
                                            <RegionDropdown
                                                country={formData.country}
                                                name="city"
                                                value={formData.city}
                                                className="form__field"
                                                onChange={(city) => setFormData({ ...formData, city })}
                                            />
                                        </div>
                                        <div className="form__group field">
                                                        <textarea
                                                            value={formData.description}
                                                            placeholder="Profile Description"
                                                            onChange={handleChange}
                                                            name="description"
                                                            className="form__field"
                                                            id="description"
                                                        />
                                            <label htmlFor="description" className="form__label">{t('profile.desc')}</label>
                                        </div>

                                        <div className="form__group field">
                                            <input
                                                type="text"
                                                placeholder="Linkedin"
                                                onChange={handleChange}
                                                name="linkLinkedin"
                                                className="form__field"
                                                value={formData.linkLinkedin}
                                                id="linkedin"
                                            />
                                            <label htmlFor="linkedin" className="form__label">Linkedin</label>
                                        </div>
                                        <div className="form__group field">
                                            <input
                                                type="text"
                                                placeholder="Facebook"
                                                onChange={handleChange}
                                                className="form__field"
                                                name="linkFacebook"
                                                value={formData.linkFacebook}
                                                id="facebook"
                                            />
                                            <label htmlFor="facebook" className="form__label">Facebook</label>
                                        </div>
                                        <div className="form__group rgpd-field field">
                                            <input
                                                type="checkbox"
                                                onChange={handleCheckbox}
                                                name="consentRegister"
                                                checked={formData.consentRegister}
                                                id="RGPD"
                                            />
                                            <label htmlFor="RGPD" className="form__label">I give consent and I agree with the terms conserning <RGPD/> </label>
                                        </div>
                                    
                                        <input type="submit" value="Register" />
                                </form>
                            </div>
                        </div>
                    )}
                </Popup>
            </form>
        </div>   
    )
    
}