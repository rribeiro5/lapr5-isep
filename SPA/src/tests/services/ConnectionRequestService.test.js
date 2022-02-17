import * as ConnectionRequestService from '../../services/ConnectionRequestService'
import apiMDRS from "../../services/apiMDRS"

import ConnectionRequest from '../../model/ConnectionRequest/ConnectionRequest'
import CreateDirectRequestDTO from '../../model/ConnectionRequest/CreateDirectRequestDTO'
import UpdateApprovalStateRequestDTO from '../../model/ConnectionRequest/UpdateApprovalStateRequestDTO'
import IntroductionRequestDTO from '../../model/ConnectionRequest/IntroductionRequestDTO'
import RequestAcceptanceDTO from '../../model/ConnectionRequest/RequestAcceptanceDTO'


jest.mock("../../services/apiMDRS")

const successData = {

    pendingAcceptanceRequests:[new ConnectionRequest("321", "1","1","2","2", "3",
        "3","message orig to dest", "message orig to inter", "message inter to dest")],


    pendingApprovalRequests: [new ConnectionRequest("321", "1","1","2","2", "3",
        "3","message orig to dest", "message orig to inter", null)],

    resultingDirectRequest: new ConnectionRequest("4321", "1","1",null,null, "3",
        "3","message orig to dest", null, null),

    resultingIntroductionRequest: new ConnectionRequest("124", "1","1","2","2", "3",
        "3","message orig to dest", "message orig to inter", null),
    
    resultingApprovalRequest: new ConnectionRequest("111", "1","1","2","2", "3",
        "3","message orig to dest", "message orig to inter", "message inter to dest"),

    resultingAcceptanceRequest: new ConnectionRequest("111", "1","1",null,null, "3",
        "3",null, null, null),


}

//-------------------------------------------------getPendingRequests--------------------------------------------------

test("getPendingRequests successfully gets 200 status code for acceptance", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData.pendingAcceptanceRequests })
    const expected = 200
    ConnectionRequestService.pendingRequests("3")
        .then(res => expect(res.status).toBe(expected))
})

test("getPendingRequests successfully gets 200 status code for approval", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData.pendingApprovalRequests })
    const expected = 200
    ConnectionRequestService.pendingRequests("2")
        .then(res => expect(res.status).toBe(expected))
})


test("getConnectionsOfUser fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ConnectionRequestService.pendingRequests("21") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//-------------------------------------------------createDirectRequest--------------------------------------------------
test("createDirectRequest successfully gets 200 status code", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: successData.resultingDirectRequest })
    const expected = 200
    ConnectionRequestService.createDirectRequest(new CreateDirectRequestDTO("1","3",
        "message orig to dest", 5, ["tag1","tag2"]))
        .then(res => expect(res.status).toBe(expected))
})


test("createDirectRequest fails for invalid user", () => {
    apiMDRS.post.mockResolvedValue({status: 200, data: {}})
    const expected = 404
    ConnectionRequestService.createDirectRequest(new CreateDirectRequestDTO("1", "23",
        "message orig to dest", 5, ["tag1", "tag2"]))
        .catch(err => expect(err.response.status).toBe(expected))
})

//-------------------------------------------------createIntroductionRequest--------------------------------------------
test("createIntroductionRequest successfully gets 200 status code", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: successData.resultingIntroductionRequest })
    const expected = 200
    ConnectionRequestService.introductionRequest(new IntroductionRequestDTO("1","2","3",
        "message orig to dest", "message orig to inter",5, ["tag1","tag2"]))
        .then(res => expect(res.status).toBe(expected))
})


test("createIntroductionRequest fails for invalid user", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: {} })
    const expected = 404
    ConnectionRequestService.introductionRequest(new IntroductionRequestDTO("1","2","33",
        "message orig to dest", "message orig to inter",5, ["tag1","tag2"]))
        .catch(err => expect(err.response.status).toBe(expected))
})

//-------------------------------------------------updateApprovalState--------------------------------------------------
test("updateApprovalState successfully gets 200 status code", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: successData.resultingApprovalRequest })
    const expected = 200
    ConnectionRequestService.updateApprovalState("111",new UpdateApprovalStateRequestDTO("111","message inter to dest", true))
        .then(res => expect(res.status).toBe(expected))
})

test("updateApprovalState successfully gets 200 status code", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: successData.resultingApprovalRequest })
    const expected = 200
    ConnectionRequestService.updateApprovalState("111",new UpdateApprovalStateRequestDTO("111","", false))
        .then(res => expect(res.status).toBe(expected))
})


test("updateApprovalState fails for invalid request id", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: {} })
    const expected = 404
    ConnectionRequestService.updateApprovalState("000",new UpdateApprovalStateRequestDTO("000","message inter to dest", true))
        .catch(err => expect(err.response.status).toBe(expected))
})

//-------------------------------------------------requestAcceptance--------------------------------------------------

test("requestAcceptance successfully gets 200 status code", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: successData.resultingAcceptanceRequest })
    const expected = 200
    ConnectionRequestService.requestAcceptance("",new RequestAcceptanceDTO(true,5, ["tag1"]))
        .then(res => expect(res.status).toBe(expected))
})

test("requestAcceptance successfully gets 200 status code", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: successData.resultingAcceptanceRequest })
    const expected = 200
    ConnectionRequestService.requestAcceptance("",new RequestAcceptanceDTO(false,0, []))
        .then(res => expect(res.status).toBe(expected))
})


test("requestAcceptance fails for invalid connection strength", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: {} })
    const expected = 404
    ConnectionRequestService.requestAcceptance("",new RequestAcceptanceDTO("test",-5, ["tag1"]))
        .catch(err => expect(err.response.status).toBe(expected))
})

