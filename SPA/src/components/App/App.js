import React,{useContext, useState} from "react";
import UnloggedContainer from '../UnloggedContainer/UnloggedContainer';
import LoggedRegion from "../LoggedRegion/LoggedRegion";

import {Context} from "../../context/loggedUser";

import './App.css';
import {ToastContainer} from "react-toastify";
import ReactFlagsSelect from "react-flags-select";
import i18next from "i18next";


function App() {
  const [lang, setLang] = useState('GB')

  const {loggedUser} = useContext(Context)
  
  const selectLang = code => {
    setLang(code)
    i18next.changeLanguage(code === 'PT' ? 'pt' : 'en')
  }

  return (
    <div className="App">
      {!loggedUser ? <UnloggedContainer/> : <LoggedRegion />
      }
      <div className="lang-select">
        <ReactFlagsSelect
            selected={lang}
            onSelect={selectLang} 
            fullWidth={false} 
            showSelectedLabel={false} 
            showSecondarySelectedLabel={false} 
            showSecondaryOptionLabel={false}
            countries={["GB", "PT"]}
            customLabels={{"GB": "EN", "PT": "PT"}}
             />
      </div>
    </div>
  );
}

export default App;
