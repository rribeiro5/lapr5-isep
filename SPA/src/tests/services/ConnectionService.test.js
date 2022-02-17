import * as ConnectionService from '../../services/ConnectionService'
import apiMDRS from "../../services/apiMDRS"

import Connection from '../../model/Connection/Connection'
import UpdateConnectionDTO from '../../model/Connection/UpdateConnectionDTO'

jest.mock("../../services/apiMDRS")

const successData = {
    connections: [new Connection("123", "1", "1", "2", "2", 5, 6, ["Futebol"])]
}

/////// getConnectionsOfUser tests

test("getConnectionsOfUser successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 200
    ConnectionService.getConnectionsOfUser("1")
        .then(res => expect(res.status).toBe(expected))
})

test("getConnectionsOfUser successfully gets connection with id 123", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = successData.connections[0].id
    ConnectionService.getConnectionsOfUser("1")
        .then(res => expect(res.data.connections[0].id).toBe(expected))
})

test("getConnectionsOfUser fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ConnectionService.getConnectionsOfUser("21") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

/////// updateConnection tests

test("updateConnection successfully updates connection and gets 200 status code", () => {
    apiMDRS.patch.mockResolvedValue({ status: 200, data: {} })
    const expected = 200
    ConnectionService.updateConnection("1", new UpdateConnectionDTO(5, ["abc"]))
        .then(res => expect(res.status).toBe(expected))
})

test("updateConnection fails for invalid connection", () => {
    apiMDRS.patch.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} }}))
    const expected = 404
    ConnectionService.updateConnection("21", new UpdateConnectionDTO(5, ["abc"])) // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

test("updateConnection fails for invalid data", () => {
    apiMDRS.patch.mockImplementation(() => Promise.reject({ response: { status: 400, data: {} }}))
    const expected = 400 // Bad Request
    ConnectionService.updateConnection("1", new UpdateConnectionDTO(-100, ["abc"])) // Invalid connection strength
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})

/////// getPossibleDestinyUsers tests

test("getPossibleDestinyUsers successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [] })
    const expected = 200
    ConnectionService.getPossibleDestinyUsers("1")
        .then(res => expect(res.status).toBe(expected))
})

test("getPossibleDestinyUsers successfully gets array of possible destiny users", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: [] })
    const expected = []
    ConnectionService.getPossibleDestinyUsers("1")
        .then(res => expect(res.data).toStrictEqual(expected))
})

test("getPossibleDestinyUsers fails for invalid user", () => {
    apiMDRS.get.mockImplementation(() => Promise.reject({ response: { status: 404, data: [] }}))
    const expected = 404
    ConnectionService.getPossibleDestinyUsers("21") // User not registered
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))
})