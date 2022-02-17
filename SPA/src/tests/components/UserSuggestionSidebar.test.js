import {act, fireEvent, render, screen} from '@testing-library/react';
import UserSugestionSidebar from "../../components/UserSugestionSidebar/UserSugestionSidebar";

import * as ConnectionService from "../../services/ConnectionService"
import React from "react";
import CreateDirectRequestDTO from "../../model/ConnectionRequest/CreateDirectRequestDTO";
import * as UserService from "../../services/UserService";
import Profile from "../../components/Profile/Profile";
import {ContextProvider} from "../../context/loggedUser";
import UserNetwork from "../../components/UserNetwork/UserNetwork";
import {getConnectionsOfUser} from "../../services/ConnectionService";
import '../../i18nextInit'

jest.mock('../../services/ConnectionService')


const suggestions={
    user: {
        name: "name"
    },
    loggedUser: "1"
}

const context = {
    loggedUser:{
        id:"1"
    }
}

test("confirm contains name heading", ()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: [] })
    render(
        <ContextProvider value={context}>
            render(<UserSugestionSidebar user={suggestions.user} />)
        </ContextProvider>
    )
    const element = screen.getByRole('heading',suggestions.user.name)
    expect(element).toBeInTheDocument()
})

test("confirm contains connection button", ()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: [] })
    render(
        <ContextProvider value={context}>
            render(<UserSugestionSidebar user={suggestions.user} />)
        </ContextProvider>
    )
    const element = screen.getByRole('button', { className: "button" })
    expect(element).toBeInTheDocument()
})

test("confirm contains message",async ()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: [] })
    render(
        <ContextProvider value={context}>
            render(<UserSugestionSidebar user={suggestions.user} />)
        </ContextProvider>
    )
    const btn = screen.getByRole('button', { className: "button" })
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Message to Destiny User')
    expect(element).toBeInTheDocument()
})

test("confirm contains Connection Strength",async ()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: [] })
    render(
        <ContextProvider value={context}>
            render(<UserSugestionSidebar user={suggestions.user} />)
        </ContextProvider>
    )
    const btn = screen.getByRole('button', { className: "button" })
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Connection Strength')
    expect(element).toBeInTheDocument()
})

test("confirm contains DirectConnectionRequestButton",async ()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: [] })
    render(
        <ContextProvider value={context}>
            render(<UserSugestionSidebar user={suggestions.user} />)
        </ContextProvider>
    )
    
    const btn = screen.getByRole('button', { className: "button" })
    fireEvent.click(btn)
    const element=screen.getAllByRole('button',{ className: "DirectConnectionRequestButton" })[1]
    expect(element).toBeInTheDocument()
})

test("confirm contains Close Button",async ()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: [] })
    render(
        <ContextProvider value={context}>
            render(<UserSugestionSidebar user={suggestions.user} />)
        </ContextProvider>
    )
    
    const btn = screen.getByRole('button', { className: "button" })
    fireEvent.click(btn)
    const element=screen.getAllByRole('button',{ className: "Close" })[0]
    expect(element).toBeInTheDocument()
})
