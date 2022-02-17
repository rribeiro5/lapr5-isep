import { act, render, screen, fireEvent } from '@testing-library/react'
import * as UserService from '../../services/UserService'
import * as ConnectionService from '../../services/ConnectionService'
import FriendsInCommon from '../../components/FriendsInCommon/FriendsInCommon'
import { ContextProvider } from '../../context/loggedUser'
import '../../i18nextInit'

jest.mock('../../services/UserService')
jest.mock('../../services/ConnectionService')

const context = {
    loggedUser:{
        id:'4'
    }
}
const destUserId = '5'

const common = [
    { id: '1', name: "a", email: "a@gmail.com", avatar: undefined },
    { id: '2', name: "b", email: "b@gmail.com", avatar: undefined },
    { id: '3', name: "c", email: "c@gmail.com", avatar: undefined }
]

const connections = {
    connections: [ { dUser: { value: '4' }, oUser: { value: '5' } } ]
}

test("render common friends button", async () => {
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: common })
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: connections })
    await act(async () => render(<ContextProvider value={context}><FriendsInCommon destUserId={destUserId} /></ContextProvider>))
    const element = screen.getByText("3 Friends In Common")
    expect(element).toBeInTheDocument()
})

test("render popup title", async () => {
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: common })
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: connections })
    await act(async () => render(<ContextProvider value={context}><FriendsInCommon destUserId={destUserId} /></ContextProvider>))
    const btn = screen.getByRole("button")
    fireEvent.click(btn)
    const element = screen.getByText("friends In Common")
    expect(element).toBeInTheDocument()
})

test("render no common friends message", async () => {
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: [] }) // Empty list of common friends
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: connections })
    await act(async () => render(<ContextProvider value={context}><FriendsInCommon destUserId={destUserId} /></ContextProvider>))
    const btn = screen.getByRole("button")
    fireEvent.click(btn)
    const element = screen.getByText("No friends in common !")
    expect(element).toBeInTheDocument()
})