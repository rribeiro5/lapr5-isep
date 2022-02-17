import apiMDRS from "./apiMDRS"

export const tagCloudAllUsers = () => {
    return apiMDRS.get(`/api/tagcloud/users`)
}

export const tagCloudAllConns = () => {
    return apiMDRS.get(`/api/tagcloud/connection`)
}

export const tagCloudOfUser = (userId) => {
    return apiMDRS.get(`/api/tagcloud/user/${userId}`)
}

export const tagCloudOfUserConns = (userId) => {
    return apiMDRS.get(`/api/tagcloud/connection/${userId}`)
}