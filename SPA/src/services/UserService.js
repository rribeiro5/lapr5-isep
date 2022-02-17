import apiMDRS from "./apiMDRS"


export const registerUser = (user)=>apiMDRS.post(`/api/users`,user)
export const login = (login)=>apiMDRS.post(`/api/users/login`,login)
export const getPrivateProfile = (id)=>apiMDRS.get(`/api/users/profile/${id}`)
export const getPrivateProfileByEmail = (email)=>apiMDRS.get(`/api/users/profile/email/${email}`)
export const updatePrivateProfile = (id, profile) => apiMDRS.put(`/api/users/profile/${id}`,profile)
export const getMutualFriends = (orig,dest) => apiMDRS.get(`/api/users/mutualFriends/${orig}/${dest}`)
export const getUserNetwork = (userId,level) => apiMDRS.get(`/api/users/network/${userId}/${level}`)

export const getUsersByName = (name) => {
    return apiMDRS.get(`/api/SearchUsers/GetByName/${name}`)
}

export const getUserByEmail = (email) => {
    return apiMDRS.get(`/api/searchusers/getbyemail/${email}`)
}

export const getUsersByCountry = (country) => {
    return apiMDRS.get(`/api/searchusers/getbycountry/${country}`)
}

export const getUsersByTags = () => {
    return apiMDRS.get(`/api/searchusers`)
}

export const getAllUsers = () =>{
    return apiMDRS.get(`/api/Users`)
}

export const getDTOSuggestedUser = (userId) => {
    return apiMDRS.get(`/api/users/suggested/${userId}`)
}

export const updateUserEmotionalState = (userId, body) => {
    return apiMDRS.patch(`/api/users/emotionalState/${userId}`, body)
}

export const getSocialNetworkDimension = () => {
    return apiMDRS.get(`/api/usernetwork/networkDimension`)
}

export const deleteUser = (userId) => {
    return apiMDRS.delete(`/api/users?id=${userId}`)
}