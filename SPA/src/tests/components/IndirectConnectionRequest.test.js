import { act, render, screen } from '@testing-library/react';
import IntroductionConnectionRequest from '../../components/IndirectConnectionRequest/IntroductionConnectionRequest'
import * as ConnectionService from '../../services/ConnectionService'
import * as UserService from '../../services/UserService'
import {ContextProvider} from "../../context/loggedUser";
import DirectRequest from "../../components/DirectRequest/DirectRequest";
import React from "react";
import '../../i18nextInit'

jest.mock('../../services/ConnectionService')
jest.mock('../../services/UserService')

const loggedUser = { id: '1' }
const destUsers = [{id: '2', birthDayDate: '', email: '2@gmail.com', name: 'User2', avatar: 'https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50', interestTags: ['abc'], emotionalState: null}]
const mutualFriends = [{id: '3', birthDayDate: '', email: '3@gmail.com', name: 'User3', avatar: 'https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50', interestTags: ['abc'], emotionalState: null}]

const context = {
    loggedUser:{
        id:"1"
    }
}
// Verify that two select element were rendered
test("render selectors", async () => {
    ConnectionService.getPossibleDestinyUsers.mockResolvedValue({ status: 200, data: destUsers })
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: mutualFriends })
    await act( async () => render(
        <ContextProvider value={context}>
            <IntroductionConnectionRequest nameCurrentComponent={() => true} />
        </ContextProvider>   )
    )
    const element = screen.getAllByRole('combobox')
    expect(element.length).toBe(2)
})

// Verify that the message to destiny input was rendered
test("render destiny input", async () => {
    ConnectionService.getPossibleDestinyUsers.mockResolvedValue({ status: 200, data: destUsers })
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: mutualFriends })
    await act( async () => render(
        <ContextProvider value={context}>
            <IntroductionConnectionRequest nameCurrentComponent={() => true} />
        </ContextProvider>   )
    )
    const element = screen.getByPlaceholderText("Message to Destiny User")
    expect(element).toBeInTheDocument()
})

// Verify that the message to intermediate input was rendered
test("render intermediate", async () => {
    ConnectionService.getPossibleDestinyUsers.mockResolvedValue({ status: 200, data: destUsers })
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: mutualFriends })
    await act( async () => render(
        <ContextProvider value={context}>
            <IntroductionConnectionRequest nameCurrentComponent={() => true} />
        </ContextProvider>   )
    )
    const element = screen.getByPlaceholderText("Message to Intermediary User")
    expect(element).toBeInTheDocument()
})

// Verify that the connection strength input was rendered
test("render connection strength input", async () => {
    ConnectionService.getPossibleDestinyUsers.mockResolvedValue({ status: 200, data: destUsers })
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: mutualFriends })
    await act( async () => render(
        <ContextProvider value={context}>
            <IntroductionConnectionRequest nameCurrentComponent={() => true} />
        </ContextProvider>   )
    )
    const element = screen.getByPlaceholderText("Connection Strength")
    expect(element).toBeInTheDocument()
})

// Verify that the tags input was rendered
test("render tags input", async () => {
    ConnectionService.getPossibleDestinyUsers.mockResolvedValue({ status: 200, data: destUsers })
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: mutualFriends })
    await act( async () => render(
        <ContextProvider value={context}>
            <IntroductionConnectionRequest nameCurrentComponent={() => true} />
        </ContextProvider>   )
    )
    const element = screen.getByPlaceholderText("Add a tag")
    expect(element).toBeInTheDocument()
})

// Verify that the submit button was rendered
test("render submit button", async () => {
    ConnectionService.getPossibleDestinyUsers.mockResolvedValue({ status: 200, data: destUsers })
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: mutualFriends })
    await act( async () => render(
        <ContextProvider value={context}>
            <IntroductionConnectionRequest nameCurrentComponent={() => true} />
        </ContextProvider>   )
    )
    const element = screen.getByRole('button', { value: /Send Request/i })
    expect(element).toBeInTheDocument()
})