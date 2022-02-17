import * as TagCloudService from '../../services/TagCloudService'
import apiMDRS from '../../services/apiMDRS'
import {tagCloudAllUsers, tagCloudOfUser, tagCloudOfUserConns} from "../../services/TagCloudService";

jest.mock('../../services/apiMDRS')


const tags = [
    { value: 'JavaScript', count: 38 },
    { value: 'React', count: 30 },
    { value: 'Nodejs', count: 28 },
    { value: 'Express.js', count: 25 },
    { value: 'HTML5', count: 33 },
    { value: 'MongoDB', count: 18 },
    { value: 'CSS3', count: 20 },
]

/////// TagCloudAllUsers tests

test("TagCloud all users successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 200
    TagCloudService.tagCloudAllUsers()
        .then(res => expect(res.status).toBe(expected))
})

test("TagCloud all users successfully gets Tags Info with length 7", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 7
    TagCloudService.tagCloudAllUsers()
        .then(res => expect(res.data.length).toBe(expected))
})

test("TagCloud successfully gets correct value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = "JavaScript"
    TagCloudService.tagCloudAllUsers()
        .then(res => expect(res.data[0].value).toBe(expected))
})

test("TagCloud successfully gets correct count value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 38
    TagCloudService.tagCloudAllUsers()
        .then(res => expect(res.data[0].count).toBe(expected))
})



/////// tagCloudAllConns tests

test("TagCloud all Connections successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 200
    TagCloudService.tagCloudAllConns()
        .then(res => expect(res.status).toBe(expected))
})

test("TagCloud all Connections successfully gets Tags Info with length 7", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 7
    TagCloudService.tagCloudAllConns ()
        .then(res => expect(res.data.length).toBe(expected))
})

test("TagCloud all connections successfully gets correct value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = "React"
    TagCloudService.tagCloudAllConns ()
        .then(res => expect(res.data[1].value).toBe(expected))
})

test("TagCloud all connections successfully gets correct count value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 30
    TagCloudService.tagCloudAllConns ()
        .then(res => expect(res.data[1].count).toBe(expected))
})

// tagCloudOfUser tests

test("TagCloud of user successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 200
    TagCloudService.tagCloudOfUser()
        .then(res => expect(res.status).toBe(expected))
})

test("TagCloud of user successfully gets Tags Info with length 7", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 7
    TagCloudService.tagCloudOfUser()
        .then(res => expect(res.data.length).toBe(expected))
})

test("TagCloud of user gets correct value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = "Nodejs"
    TagCloudService.tagCloudOfUser()
        .then(res => expect(res.data[2].value).toBe(expected))
})

test("TagCloud of user gets correct count value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 28
    TagCloudService.tagCloudOfUser ()
        .then(res => expect(res.data[2].count).toBe(expected))
})


// tagCloudOfConnectionsUser tests

test("TagCloud of connections of user successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 200
    TagCloudService.tagCloudOfUserConns()
        .then(res => expect(res.status).toBe(expected))
})

test("TagCloud of connections user successfully gets Tags Info with length 7", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 7
    TagCloudService.tagCloudOfUserConns()
        .then(res => expect(res.data.length).toBe(expected))
})

test("TagCloud of connections of user gets correct value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = "Express.js"
    TagCloudService.tagCloudOfUserConns()
        .then(res => expect(res.data[3].value).toBe(expected))
})

test("TagCloud of connections of user gets correct count value", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: tags })
    const expected = 25
    TagCloudService.tagCloudOfUserConns ()
        .then(res => expect(res.data[3].count).toBe(expected))
})