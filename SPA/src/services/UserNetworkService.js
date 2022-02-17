import apiMDRS from "./apiMDRS"

export const userNetworkStrength = (userId) => {
    return apiMDRS.get(`/api/UserNetwork/NetworkConnectionStrength/${userId}`)
}

export const userNetworkSize = (userId,level) =>{
    return apiMDRS.get(`/api/UserNetwork/networkSize/${userId}/${level}`)
}

export const getGroupSuggestions = (groupSuggestionDto) => {
    return apiMDRS.post(`/api/userNetwork/GroupSuggestions`,groupSuggestionDto)
}