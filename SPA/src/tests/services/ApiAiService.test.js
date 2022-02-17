import * as ApiAIService from '../../services/ApiAIService'
import apiAI from "../../services/apiAI"
import CreatingUserDto from "../../model/User/CreatingUserDto";
import PrivateProfileDTO from "../../model/Profile/PrivateProfileDTO";


jest.mock("../../services/apiAI")

const successData = {
    traject: {
        caminho:["1@gmail.com",
            "2@gmail.com"],
        forcaResultante: 9
    }
}

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


/////// getSafestTrajectUnidirecional

test("getSafestTrajectUnidirecional successfully gets 200 status code", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject })
    const expected = 200
    ApiAIService.getSafestTrajectUnidirecional("1@gmail.com","2@gmail.com",1)
        .then(res => expect(res.status).toBe(expected))
})


test("getSafestTrajectUnidirecional successfully gets traject with 2 length", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject  })
    const expected = successData.traject.caminho.length
    ApiAIService.getSafestTrajectUnidirecional("1@gmail.com","2@gmail.com",1)
        .then(res => expect(res.data.caminho.length).toBe(expected))
})

test("getSafestTrajectUnidirecional fails for invalid user", () => {
    apiAI.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ApiAIService.getSafestTrajectUnidirecional("invalid@gmail.com","invalid2@gmail.com") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

/// getSafestTrajectBidirecional

test("getSafestTrajectBidirecional successfully gets 200 status code", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject })
    const expected = 200
    ApiAIService.getSafestTrajectBidirecional("1@gmail.com","2@gmail.com",1)
        .then(res => expect(res.status).toBe(expected))
})


test("getSafestTrajectBidirecional successfully gets traject with 2 length", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject  })
    const expected = successData.traject.caminho.length
    ApiAIService.getSafestTrajectBidirecional("1@gmail.com","2@gmail.com",1)
        .then(res => expect(res.data.caminho.length).toBe(expected))
})

test("getSafestTrajectBidirecional fails for invalid user", () => {
    apiAI.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ApiAIService.getSafestTrajectBidirecional("invalid@gmail.com","invalid2@gmail.com") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})


///  getStrongestTrajectUnidirecional

test(" getStrongestTrajectUnidirecional successfully gets 200 status code", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject })
    const expected = 200
    ApiAIService.getStrongestTrajectUnidirecional("1@gmail.com","2@gmail.com")
        .then(res => expect(res.status).toBe(expected))
})


test("getStrongestTrajectUnidirecional successfully gets traject with 2 length", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject  })
    const expected = successData.traject.caminho.length
    ApiAIService.getStrongestTrajectUnidirecional("1@gmail.com","2@gmail.com")
        .then(res => expect(res.data.caminho.length).toBe(expected))
})

test("getStrongestTrajectUnidirecional fails for invalid user", () => {
    apiAI.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ApiAIService.getStrongestTrajectUnidirecional("invalid@gmail.com","invalid2@gmail.com") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})


///  getStrongestTrajectBidirecional

test(" getStrongestTrajectbidirecional successfully gets 200 status code", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject })
    const expected = 200
    ApiAIService.getStrongestTrajectBidirecional("1@gmail.com","2@gmail.com")
        .then(res => expect(res.status).toBe(expected))
})


test("getStrongestTrajectBidirecional successfully gets traject with 2 length", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject  })
    const expected = successData.traject.caminho.length
    ApiAIService.getStrongestTrajectBidirecional("1@gmail.com","2@gmail.com")
        .then(res => expect(res.data.caminho.length).toBe(expected))
})

test("getStrongestTrajectBidirecional fails for invalid user", () => {
    apiAI.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ApiAIService.getStrongestTrajectBidirecional("invalid@gmail.com","invalid2@gmail.com") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})


/////// getSuggested tests

test("getSuggested successfully gets 200 status code", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: [] })
    const expected = 200
    ApiAIService.getSuggestedUser(profileDTO.id)
        .then(res => expect(res.status).toBe(expected))
})

test("getSuggested successfully gets array of possible destiny users", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: [] })
    const expected = []
    ApiAIService.getSuggestedUser(profileDTO.id)
        .then(res => expect(res.data).toStrictEqual(expected))
})

test("getPossibleDestinyUsers fails for invalid user", () => {
    apiAI.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: [] }}))
    const expected = 404
    ApiAIService.getSuggestedUser("-1")// User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

///  getShortest

test(" getShortestTraject successfully gets 200 status code", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject })
    const expected = 200
    ApiAIService.getShortestTraject("1@gmail.com","2@gmail.com")
        .then(res => expect(res.status).toBe(expected))
})


test(" getShortestTraject successfully gets traject with 2 length", () => {
    apiAI.get.mockResolvedValue({ status: 200, data: successData.traject  })
    const expected = successData.traject.caminho.length
    ApiAIService.getShortestTraject("1@gmail.com","2@gmail.com")
        .then(res => expect(res.data.caminho.length).toBe(expected))
})

test("getShortestTraject fails for invalid user", () => {
    apiAI.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ApiAIService.getShortestTraject("invalid@gmail.com","invalid2@gmail.com") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})
