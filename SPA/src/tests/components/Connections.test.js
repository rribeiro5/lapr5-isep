import * as ConnectionService from '../../services/ConnectionService'
import * as UserService from '../../services/UserService'
import { act, render, screen, fireEvent } from '@testing-library/react'
import { ContextProvider } from '../../context/loggedUser'
import '../../i18nextInit'
import Connections from "../../components/Connections/Connections";


jest.mock('../../services/ConnectionService')
jest.mock('../../services/UserService')

const context = {
    loggedUser:{
        id:'4'
    }
}

const destUserId = '5'


const data = {
    connections: [ { 
        id:"865-321", oUser:{ id:1 } , origUser:"1" , dUser: { id:"2" } ,destUser:"2" , connectionStrength :5, relationshipStrength: 2, tags:["porto","benfica"]
    } ]
}

const common = [
    { id: '1', name: "a", email: "a@gmail.com", avatar: undefined },
    { id: '2', name: "b", email: "b@gmail.com", avatar: undefined },
    { id: '3', name: "c", email: "c@gmail.com", avatar: undefined }
]

test("Test Connection Strength is 5 ", async () => {
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: [] })
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: data })
    
    await act(async () => render(<ContextProvider value={context}>
        <Connections nameCurrentComponent={()=>true} />
    </ContextProvider>))
    
    const element = screen.getByText("Connection Strength: 5")
    expect(element).toBeInTheDocument()
})

test("Test Relationship Strength is 2 ", async () => {
    UserService.getMutualFriends.mockResolvedValue({ status: 200, data: [] })
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: data })

    await act(async () => render(<ContextProvider value={context}>
        <Connections nameCurrentComponent={()=>true} />
    </ContextProvider>))

    const element = screen.getByText("Relationship Strength: 2")
    expect(element).toBeInTheDocument()
})