import * as UserService from "../../services/ConnectionRequestService";
import {act, fireEvent, render, screen} from "@testing-library/react";
import * as ConnectionRequestService from "../../services/ConnectionRequestService";
import UpdateConnection from "../../components/UpdateConnection/UpdateConnection";
import {ContextProvider} from "../../context/loggedUser";
import DirectConnectionRequest from "../../components/DirectConnectionRequest/DirectConnectionRequest";
import React from "react";
import '../../i18nextInit'

jest.mock('../../services/ConnectionRequestService')

const context = {
    loggedUser:{
        id:"1"
    }
}

//orig={currentUserId} dest={response}
const response={}

test("page renders button",async ()=>{
    ConnectionRequestService.createDirectRequest.mockResolvedValue({ status: 200, data: [] })
    const user = jest.fn();
    await act( async () => render(
        <ContextProvider value={{user}}>
            <DirectConnectionRequest  dest={response} />
        </ContextProvider>
    ))
    const element = screen.getByRole('button', { className:"DirectConnectionRequestButton" })
    expect(element).toBeInTheDocument()
})

test("page renders header",async ()=>{
    ConnectionRequestService.createDirectRequest.mockResolvedValue({ status: 200, data: [] })
    const user = jest.fn();
    await act( async () => render(
        <ContextProvider value={{user}}>
            <DirectConnectionRequest  dest={response} />
        </ContextProvider>
    ))
    const btn = screen.getByRole('button', { className:"DirectConnectionRequestButton" })
    fireEvent.click(btn)
    const element=screen.getByText('Direct Connection Request')
    expect(element).toBeInTheDocument()
})


test("page renders text input for destiny user",async ()=>{
    ConnectionRequestService.createDirectRequest.mockResolvedValue({ status: 200, data: [] })
    const user = jest.fn();
    await act( async () => render(
        <ContextProvider value={{user}}>
            <DirectConnectionRequest  dest={response} />
        </ContextProvider>
    ))
    const btn = screen.getByRole('button', { className:"DirectConnectionRequestButton" })
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Message to Destiny User')
    expect(element).toBeInTheDocument()
})

test("page renders text input for connection strengh",async ()=>{
    ConnectionRequestService.createDirectRequest.mockResolvedValue({ status: 200, data: [] })
    const user = jest.fn();
    await act( async () => render(
        <ContextProvider value={{user}}>
            <DirectConnectionRequest  dest={response} />
        </ContextProvider>
    ))
    const btn = screen.getByRole('button', { className:"DirectConnectionRequestButton" })
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Connection Strength')
    expect(element).toBeInTheDocument()
})