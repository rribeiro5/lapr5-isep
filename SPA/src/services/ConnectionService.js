import apiMDRS from "./apiMDRS"

export const getConnectionsOfUser = (userId) => {
    return apiMDRS.get(`/api/connections/user/${userId}`)
}

export const updateConnection = (connId, newData) => {
    return apiMDRS.patch(`/api/connections/${connId}`, newData)
}

export const getPossibleDestinyUsers = (userId) =>{
    return apiMDRS.get(`/api/connections/destinyUsers/${userId}`)
}