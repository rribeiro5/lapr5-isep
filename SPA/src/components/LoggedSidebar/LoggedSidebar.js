
import React, {useContext} from 'react'
import { ProSidebar,SidebarHeader,SidebarFooter,SidebarContent,Menu,MenuItem,SubMenu } from 'react-pro-sidebar';
import 'react-pro-sidebar/dist/css/styles.css';
import {SiGraphql,SiGamejolt} from 'react-icons/si';
import {FaUserFriends,FaRegBell} from 'react-icons/fa';
import {GiStonePath,GiWireframeGlobe} from 'react-icons/gi';
import {HiLogout} from 'react-icons/hi';
import {RiArrowDropDownLine,RiUserLine,RiLinksFill} from 'react-icons/ri'
import {MdFilterList,MdFilter2,MdOutlineLeaderboard,MdGroups} from 'react-icons/md'
import {NavLink} from "react-router-dom"
import './LoggedSidebar.css';
import {Context} from "../../context/loggedUser";
import { useTranslation } from 'react-i18next';
import { BsTags } from 'react-icons/bs';

export default function LoggedSidebar(){

    const {setLoggedUser} = useContext(Context)
    
    const { t } = useTranslation()
    
    return (
        <ProSidebar>
            <SidebarHeader>
                <NavLink to="/" activeclassname="selected"><h1 className="SidebarHeaderTitle"> GRAPHS4SOCIAL </h1></NavLink>
            </SidebarHeader>
            
            <SidebarContent>

                {/* Pedidos */}
                <Menu >
                    <MenuItem icon={<FaRegBell />} > 
                        <NavLink to="/incomingRequests" activeclassname="selected">{t('sidebar.increqs')}</NavLink>
                    </MenuItem>
                </Menu>

                {/* Editar Connections*/}

                <Menu >
                    <MenuItem icon={<RiLinksFill />} >
                        <NavLink to="/connections" activeclassname="selected">{t('sidebar.conns')}</NavLink>
                    </MenuItem>
                </Menu>

                {/* Pedidos de connection */}

                <SubMenu title={t('sidebar.connreq')} suffix={<RiArrowDropDownLine />} icon={<FaUserFriends />}>
                    <MenuItem icon={<MdFilterList />}>
                        <NavLink to="/directRequest" activeclassname="selected">{t('sidebar.dirreq')}</NavLink>
                    </MenuItem>

                    <MenuItem icon={<MdFilter2 />}>
                        <NavLink to="/introductionRequest" activeclassname="selected">{t('sidebar.introduction')}</NavLink>
                    </MenuItem>
                </SubMenu>

                {/* Group Suggestion */}
                <Menu >
                    <MenuItem icon={<MdGroups />} >
                        <NavLink to="/groupSuggestions" activeclassname="selected">{t('sidebar.groupSuggestions')}</NavLink>
                    </MenuItem>
                </Menu>
                
                
                {/* Trajectos */}
                <Menu >
                    <MenuItem icon={<GiStonePath />} >
                        <NavLink to="/trajects" activeclassname="selected">{t('sidebar.trajects')}</NavLink>
                    </MenuItem>
                </Menu>
                
                {/* Rede */}
                <Menu >
                    <MenuItem icon={<SiGraphql />} >
                        <NavLink to="/network" activeclassname="selected">{t('sidebar.checkntw')}</NavLink>
                    </MenuItem>
                </Menu>


                {/* Visualizar Network */}
                <Menu >
                    <MenuItem icon={<GiWireframeGlobe />} >
                        <NavLink to="/visualNetwork" activeclassname="selected">{t('sidebar.visuntw')}</NavLink>
                    </MenuItem>
                </Menu>
                

                {/* Leaderboard */}
                <Menu >
                    <MenuItem icon={<MdOutlineLeaderboard />}>
                        <NavLink to="/leaderboard" activeclassname="selected">{t('sidebar.leaderboard')}</NavLink>
                    </MenuItem>
                </Menu>
              
                {/* TagCloud */}
                <Menu >
                    <MenuItem icon={<BsTags />}>
                        <NavLink to="/tagcloud" activeclassname="selected">{t('sidebar.tagcloud')}</NavLink>
                    </MenuItem>
                </Menu>

                {/* Perfil */}
                <Menu >
                    <MenuItem icon={<RiUserLine />} >
                        <NavLink to="/profile" activeclassname="selected">{t('sidebar.profile')}</NavLink>
                    </MenuItem>
                </Menu>
                
            </SidebarContent>
            <SidebarFooter>
                <Menu>
                    <MenuItem onClick={()=>setLoggedUser(undefined)} icon={<HiLogout />}>
                        <NavLink to="/" activeclassname="selected">{t('sidebar.logout')}</NavLink>
                    </MenuItem>
                </Menu>
            </SidebarFooter>
        </ProSidebar>
    )
}