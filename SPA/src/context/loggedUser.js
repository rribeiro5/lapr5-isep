import React, {useState, useContext, createContext} from "react";

export const Context = createContext();

export const ContextProvider = (props)=>{
    const [loggedUser,setLoggedUser] = useState(props?.value?.loggedUser)
    
    const value = {
        loggedUser,
        setLoggedUser
    }
    
    return <Context.Provider value={value} > {props.children} </Context.Provider>
}