import React,{useState} from 'react';
import './MainContainer.css'


import Profile from '../Profile/Profile'
import IncomingRequests from "../IncomingRequests/IncomingRequests";
import IntroductionConnectionRequest from "../IndirectConnectionRequest/IntroductionConnectionRequest";
import DirectRequest from "../DirectRequest/DirectRequest"
import { Routes, Route} from "react-router-dom";
import TrajectsContainer from "../TrajectsContainer/TrajectsContainer";
import UserNetwork from "../UserNetwork/UserNetwork";
import Leaderboard from "../Leaderboard/Leaderboard";
import Graph3D from "../VisualizerNetwork/Graph3D";
import Connections from '../Connections/Connections';
import TagCloud from '../TagCloud/TagCloud';
import GroupSuggestions from "../GroupSuggestions/GroupSuggestions";

export default function MainContainer(){
    
    const [state,updateState] = useState({
        currentComponent:"Profile"
    })
    
    function changeNameCurrentComponent(name){
        updateState({currentComponent: name})
    }
    
    
    return(
        <main className="MainContainer">
            <div className="TitleContainer">
                <span className="Title">{state.currentComponent}</span>
            </div>

            
            <div className="UseCaseContainer">

                <Routes>
                    <Route path="/" element={  <Profile nameCurrentComponent={changeNameCurrentComponent}/>} />
                    <Route path="/profile/:email" element={<Profile nameCurrentComponent={changeNameCurrentComponent} />} />
                    <Route path="/profile" element={<Profile nameCurrentComponent={changeNameCurrentComponent}/>} />
                    <Route path="/introductionRequest" element={<IntroductionConnectionRequest nameCurrentComponent={changeNameCurrentComponent}/>} />
                    <Route path="/incomingRequests" element={<IncomingRequests nameCurrentComponent={changeNameCurrentComponent}/>} />
                    <Route path="/visualNetwork" element={<Graph3D  />} />
                    <Route path="/leaderboard" element={<Leaderboard nameCurrentComponent={changeNameCurrentComponent}/>} />
                    <Route path="/tagcloud" element={<TagCloud nameCurrentComponent={changeNameCurrentComponent}/>} />
                    <Route path="/connections" element={<Connections nameCurrentComponent={changeNameCurrentComponent}/>}/>
                    <Route path="/trajects" element={<TrajectsContainer nameCurrentComponent={changeNameCurrentComponent} />} />
                    <Route path="/directRequest" element={<DirectRequest nameCurrentComponent={changeNameCurrentComponent} />}/>
                    <Route path="/network" element={<UserNetwork nameCurrentComponent={changeNameCurrentComponent}/>}/>
                    <Route path="/groupSuggestions" element={<GroupSuggestions nameCurrentComponent={changeNameCurrentComponent}/>}/>
                    <Route path="*" element={<Profile nameCurrentComponent={changeNameCurrentComponent}/>}/>
                    
                </Routes>
     
            </div>
            
            
        </main>
    )
}