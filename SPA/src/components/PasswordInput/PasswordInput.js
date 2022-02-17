import React,{useState} from 'react';
import NiceInputPassword from 'react-nice-input-password';
import 'react-nice-input-password/dist/react-nice-input-password.css';
import {TextField, InputLabel, Typography} from '@material-ui/core';
import LockIcon from '@material-ui/icons/Lock';
import './PasswordInput.css'

export default function PasswordInput(props){

    const[visible,changeVisible] = useState(false)

    let{placeholder,value,name,onChange} = props;

    
    function handlePassword(data){
        const {name,value} = data
        onChange(prevData =>{
            return {
                ...prevData,
                [name]:value
            }
        })
    }



    return(
        <NiceInputPassword
            placeholder={placeholder}
            name={name}
            value={value}
            showSecurityLevelBar
            visible={visible}
            LabelComponent={InputLabel}
            InputComponent={TextField}
            InputComponentProps={{
                variant: 'outlined',
                InputProps: {
                endAdornment: <LockIcon onClick={()=>{changeVisible(prevData=>!prevData)}} />,
                }
            }}
            securityLevels={[
            {
                descriptionLabel: <Typography>At least 1 Number</Typography>,
                validator: /.*[0-9].*/,
            },
            {
                descriptionLabel: <Typography>At least 1 lowercase letter</Typography>,
                validator: /.*[a-z].*/,
            },
            {
                descriptionLabel:<Typography>At least 1 uppercase letter</Typography>,
                validator: /.*[A-Z].*/,
            },
            ]}
            required
            onChange={handlePassword}
        />
    )

}