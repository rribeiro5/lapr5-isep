import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './components/App/App';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter } from "react-router-dom";

import {ContextProvider} from "./context/loggedUser";

import './i18nextInit'

ReactDOM.render(
  <React.StrictMode>
      <BrowserRouter value={undefined}>
          <ContextProvider>
              <App />
          </ContextProvider>
      </BrowserRouter>    
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
