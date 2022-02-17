import * as UserNetworkSevice from '../../services/UserNetworkService'
import apiMDRS from '../../services/apiMDRS'

jest.mock('../../services/apiMDRS')

const successData = { value: 10 }

/////// userNetworkStrength tests

test("userNetworkStrength successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 200
    UserNetworkSevice.userNetworkStrength("123")
        .then(res => expect(res.status).toBe(expected))
})

test("userNetworkStrength successfully gets strength for user 123", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 10
    UserNetworkSevice.userNetworkStrength("123")
        .then(res => expect(res.data.value).toBe(expected))
})

test("userNetworkStrength fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    UserNetworkSevice.userNetworkSize("21") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

/////// userNetworkSize tests

test("userNetworkSize successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 200
    UserNetworkSevice.userNetworkSize("123", 3)
        .then(res => expect(res.status).toBe(expected))
})

test("userNetworkSize successfully gets strength for user 123", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 10
    UserNetworkSevice.userNetworkSize("123", 3)
        .then(res => expect(res.data.value).toBe(expected))
})

test("userNetworkSize fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    UserNetworkSevice.userNetworkSize("21", 3) // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})