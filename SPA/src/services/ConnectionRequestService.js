import apiMDRS from "./apiMDRS"

export const requestAcceptance = (reqId, answer) => {
    return apiMDRS.patch(`/api/connectionrequest/acceptance/${reqId}`, answer)
}

export const introductionRequest = (introductionRequestDTO) => {
    return apiMDRS.post(`/api/connectionrequest/introductionRequest`,introductionRequestDTO)
}

export const updateApprovalState = (reqId, body) => {
    return apiMDRS.patch(`/api/connectionrequest/approval/${reqId}`,body)
}

export const createDirectRequest = (request) => {
    return apiMDRS.post(`/api/connectionrequest/directconnection`, request)
}


export const pendingRequests = (userId) =>{
    return apiMDRS.get(`/api/connectionRequest/user/${userId}`)
}