import * as connectionRequestService from '../../services/ConnectionRequestService'
import {act, fireEvent, render, screen} from "@testing-library/react";
import IncomingAprovalRequest from "../../components/IncomingAprovalRequest/IncomingAprovalRequest";
import IncomingAcceptanceRequest from "../../components/IncomingAcceptanceRequest/IncomingAcceptanceRequest";
import React from "react";
import * as ConnectionRequestService from "../../services/ConnectionRequestService";
import {ContextProvider} from "../../context/loggedUser";
import Profile from "../../components/Profile/Profile";
import * as ConnectionService from "../../services/ConnectionService";
import '../../i18nextInit'

jest.mock('../../services/ConnectionRequestService')
jest.mock('../../services/ConnectionService')

const incomingRequests = {"id":"ffb97fd4-26c1-42d6-a778-7ae1d3eca1f5","origUser":{"value":"0e68e9a2-c8ad-4e2a-b5be-831a6ca64490"},"oUser":{"id":"0e68e9a2-c8ad-4e2a-b5be-831a6ca64490","birthDayDate":"2000/1/1","email":"3@gmail.com","name":"Rafael Ribeiro","avatar":"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRWHGFgsFsWZiRlyz4YWqGBvoNeeWmXRA-T5Q&usqp=CAU","interestTags":["tag1","tag2"],"emotionalState":null},"interUser":null,"iUser":null,"destUser":{"value":"8cbce14b-e9e2-4ae4-98ed-0d79eb7e55db"},"dUser":{"id":"8cbce14b-e9e2-4ae4-98ed-0d79eb7e55db","birthDayDate":"2000/1/1","email":"2@gmail.com","name":"Petra Pisco","avatar":"http://www.gravatar.com/avatar/a16a38cdfe8b2cbd38e8a56ab93238d3","interestTags":["tag1","tag2"],"emotionalState":null},"messageOrigToDest":"Teste","messageOrigToInter":null,"messageInterToDest":null}

const context = {
    loggedUser:{
        id:"1"
    }
}

test( "Approval renders",async()=>{
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: {connections:[]} })
    connectionRequestService.pendingRequests.mockResolvedValue({ status: 200, data: incomingRequests })
    await act( async () => render(
        <ContextProvider value={context}>
            <IncomingAprovalRequest request={incomingRequests} />
        </ContextProvider>    ))
    
    const element = screen.getByTitle('Rafael Ribeiro', { title: "Rafael Ribeiro" })
    expect(element).toBeInTheDocument()
})

// Verify requests were loaded to select 
test("render selector option", async () => {
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: {connections:[]} })
    connectionRequestService.pendingRequests.mockResolvedValue({ status: 200, data: incomingRequests })
    await act( async () => render(
        <ContextProvider value={context}>
            <IncomingAprovalRequest request={incomingRequests} />
        </ContextProvider>    ))
    const element =screen.getByTitle('Rafael Ribeiro', { title: "Rafael Ribeiro" })
    expect(element.innerHTML).toBe("<div style=\"display: table; table-layout: fixed; width: 100%; height: 100%;\"><span style=\"display: table-cell; vertical-align: middle; font-size: 100%; white-space: nowrap;\"><span></span></span></div>")
})

// Verify accept input was rendered
test("render accept input", async () => {
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: {connections:[]} })
    ConnectionRequestService.pendingRequests.mockResolvedValue({ status: 200, data: { incomingRequests } })
    await act( async () => render(
        <ContextProvider value={context}>
            <IncomingAprovalRequest request={incomingRequests} />
        </ContextProvider>    ))
    const btn = screen.getAllByRole("button",{className:"buttons"})[0]
    expect(btn).toBeInTheDocument()
})

// Verify reject input was rendered
test("render reject input", async () => {
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: {connections:[]} })
    connectionRequestService.pendingRequests.mockResolvedValue({ status: 200, data: incomingRequests })
    await act( async () => render(
        <ContextProvider value={context}>
            <IncomingAprovalRequest request={incomingRequests} />
        </ContextProvider>    ))
    const btn = screen.getAllByRole("button",{className:"buttons"})[1]
    expect(btn).toBeInTheDocument()
})


// Verify submit button was rendered
test("render submit button input", async () => {
    ConnectionService.getConnectionsOfUser.mockResolvedValue({ status: 200, data: {connections:[]} })
    connectionRequestService.pendingRequests.mockResolvedValue({ status: 200, data: incomingRequests })
    await act( async () => render(
        <ContextProvider value={context}>
            <IncomingAprovalRequest request={incomingRequests} />
        </ContextProvider>    ))
    const btn = screen.getAllByRole("button",{className:"buttons"})[0]
    fireEvent.click(btn)
    const element = screen.getAllByRole('button')[0]
    expect(element).toBeInTheDocument()
})