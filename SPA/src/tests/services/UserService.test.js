import * as UserService from '../../services/UserService'
import apiMDRS from "../../services/apiMDRS"

import PrivateProfileDTO from '../../model/Profile/PrivateProfileDTO'
import LoginDto from "../../model/User/LoginDto";
import CreatingUserDto from "../../model/User/CreatingUserDto";

jest.mock("../../services/apiMDRS")

const id = "1"
const avatar = "avatar.png"
const name = "name"
const email = "email"
const phoneNumber = "phoneNumber"
const birthdayDate = "birthdayDate"
const city = "city"
const country = "country"
const description = "description"
const points = "points"
const linkedInURL = "linkedInUrl"
const facebookURL = "facebookUrl"
const interestTags = "interestTags"
const emotionalState = "neutral"



const profileDTO = new PrivateProfileDTO(
    id, avatar, name, email, phoneNumber,
    birthdayDate, city, country, description, points,
    linkedInURL, facebookURL, interestTags, emotionalState)

const password="Password1?"

const loginDTO = new LoginDto(email, password)

const registerUserDto = new CreatingUserDto(email, phoneNumber,phoneNumber,avatar,
    city, country,linkedInURL, facebookURL, password, birthdayDate,description,interestTags)

const common = [
    { id: '1', name: "a", email: "a@gmail.com", avatar: undefined },
    { id: '2', name: "b", email: "b@gmail.com", avatar: undefined },
    { id: '3', name: "c", email: "c@gmail.com", avatar: undefined }
]

//registerUser
test("register successfully gets 200 status code", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: registerUserDto })
    const expected = 200
    UserService.registerUser(registerUserDto)
        .then(res => expect(res.status).toBe(expected))
})

test("user successfully registers", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: registerUserDto })
    const expected = registerUserDto.email
    UserService.registerUser(registerUserDto)
        .then(res => expect(res.data.email).toBe(expected))
})

test("register fails for invalid user", () => {
    apiMDRS.post.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.registerUser(null)
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//login
test("login successfully gets 200 status code", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: loginDTO })
    const expected = 200
    UserService.login(loginDTO)
        .then(res => expect(res.status).toBe(expected))
})

test("login successfully logins", () => {
    apiMDRS.post.mockResolvedValue({ status: 200, data: loginDTO })
    const expected = loginDTO.email
    UserService.login(loginDTO)
        .then(res => expect(res.data.email).toBe(expected))
})

test("login fails for invalid user", () => {
    apiMDRS.post.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.login(loginDTO)
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getPrivateProfile
test("getPrivateProfile successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = 200
    UserService.getPrivateProfile(id)
        .then(res => expect(res.status).toBe(expected))
})

test("getPrivateProfile successfully gets profile", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = profileDTO.id
    UserService.getPrivateProfile(id)
        .then(res => expect(res.data.id).toBe(expected))
})

test("updatePrivateProfile fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getPrivateProfile(id)
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//updatePrivateProfile
test("updatePrivateProfile successfully gets 200 status code", () => {
    apiMDRS.put.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = 200
    UserService.updatePrivateProfile(id, profileDTO)
        .then(res => expect(res.status).toBe(expected))
})

test("updatePrivateProfile successfully updates profile", () => {
    apiMDRS.put.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = profileDTO.id
    UserService.updatePrivateProfile(id, profileDTO)
        .then(res => expect(res.data.id).toBe(expected))
})

test("updatePrivateProfile fails for invalid user", () => {
    apiMDRS.put.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.updatePrivateProfile(id, profileDTO)
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getUserNetwork
test("getUserNetwork successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = 200
    UserService.getUserNetwork(id, 1)
        .then(res => expect(res.status).toBe(expected))
})

test("getUserNetwork successfully gets network", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = profileDTO.id
    UserService.getUserNetwork("1",1)
        .then(res => expect(res.data[0].id).toBe(expected))
})

test("getUserNetwork fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getUserNetwork()
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getUsersByName
test("getUsersByName successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = 200
    UserService.getUsersByName(name)
        .then(res => expect(res.status).toBe(expected))
})

test("getUsersByName successfully gets user with name as name", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = profileDTO.name
    UserService.getUsersByName(name)
        .then(res => expect(res.data[0].name).toBe(expected))
})

test("getUsersByName fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getUsersByName()
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getUserByEmail
test("getUserByEmail successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = 200
    UserService.getUserByEmail(email)
        .then(res => expect(res.status).toBe(expected))
})

test("getUserByEmail successfully gets user with email as email", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: profileDTO})
    const expected = profileDTO.email
    UserService.getUserByEmail(email)
        .then(res => expect(res.data.email).toBe(expected))
})

test("getUserByEmail fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getUserByEmail()
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getUsersByCountry
test("getUsersByCountry successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = 200
    UserService.getUsersByCountry(country)
        .then(res => expect(res.status).toBe(expected))
})

test("getUsersByCountry successfully gets user with country as country", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = profileDTO.country
    UserService.getUsersByCountry(country)
        .then(res => expect(res.data[0].country).toBe(expected))
})

test("getUsersByCountry fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getUsersByCountry()
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getUsersByTags
test("getUsersByTags successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = 200
    UserService.getUsersByTags()
        .then(res => expect(res.status).toBe(expected))
})

test("getUsersByTags successfully gets users with id1", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = profileDTO.tags
    UserService.getUsersByTags()
        .then(res => expect(res.data[0].tags).toBe(expected))
})

test("getUsersByTags fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getUsersByTags()
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})



//getAllUsers
test("getAllUsers successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = 200
    UserService.getAllUsers()
        .then(res => expect(res.status).toBe(expected))
})

test("getAllUsers successfully gets users with id1", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [profileDTO] })
    const expected = profileDTO.name
    UserService.getAllUsers()
        .then(res => expect(res.data[0].name).toBe(expected))
})

test("getAllUsers fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getAllUsers()
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})


//getDTOSuggestedUser
test("getDTOSuggestedUser successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = 200
    UserService.getDTOSuggestedUser("1")
        .then(res => expect(res.status).toBe(expected))
})

test("getDTOSuggestedUser successfully gets suggested user with id 1", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = profileDTO.name
    UserService.getDTOSuggestedUser("1")
        .then(res => expect(res.data.name).toBe(expected))
})

test("getDTOSuggestedUser fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getDTOSuggestedUser("21") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//update emotional state
test("updateUserEmotionalState successfully updates connection and gets 200 status code", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: profileDTO })
    const expected = 200
    UserService.updateUserEmotionalState(id, profileDTO)
        .then(res => expect(res.status).toBe(expected))
})

test("updateUserEmotionalState fails for invalid connection", () => {
    apiMDRS.patch.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.updateUserEmotionalState("21", profileDTO) // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

test("updateUserEmotionalState fails for invalid data", () => {
    apiMDRS.patch.mockImplementation(() => Promise.reject({ response: { status: 400, data: {} } }))
    const expected = 400 // Bad Request
    UserService.updateUserEmotionalState("1", new PrivateProfileDTO(null)) // Invalid dto
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

//getMutualFriends
test("getMutualFriends successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: common })
    const expected = 200
    UserService.getMutualFriends("4", "5")
        .then(res => expect(res.status).toBe(expected))
})

test("getMutualFriends successfully gets common friends", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: common })
    const expected = common[0].id
    UserService.getMutualFriends("4", "5")
        .then(res => expect(res.data[0].id).toBe(expected))
})

test("getMutualFriends fails for invalid users", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected = 404
    UserService.getMutualFriends("21", "31") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})