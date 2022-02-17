import * as LeaderboardService from '../../services/LeaderboardService'
import apiMDRS from '../../services/apiMDRS'

jest.mock('../../services/apiMDRS')

const successData = [
    { id: '1', name: "a", email: "a@gmail.com", avatar: undefined, value: 30 },
    { id: '2', name: "b", email: "b@gmail.com", avatar: undefined, value: 20 },
    { id: '3', name: "c", email: "c@gmail.com", avatar: undefined, value: 10 }
]

/////// leaderboardNetworkDimension tests

test("leaderboardNetworkDimension successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 200
    LeaderboardService.leaderboardNetworkDimension()
        .then(res => expect(res.status).toBe(expected))
})

test("leaderboardNetworkDimension successfully gets dimension leaderboard with length 3", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 3
    LeaderboardService.leaderboardNetworkDimension()
        .then(res => expect(res.data.length).toBe(expected))
})

test("leaderboardNetworkDimension successfully gets dimension leaderboard with value 30", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 30
    LeaderboardService.leaderboardNetworkDimension()
        .then(res => expect(res.data[0].value).toBe(expected))
})

/////// leaderboardNetworkStronghold tests

test("leaderboardNetworkStronghold successfully gets 200 status code", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 200
    LeaderboardService.leaderboardNetworkStronghold()
        .then(res => expect(res.status).toBe(expected))
})

test("leaderboardNetworkStronghold successfully gets stronghold leaderboard with length 3", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 3
    LeaderboardService.leaderboardNetworkStronghold()
        .then(res => expect(res.data.length).toBe(expected))
})

test("leaderboardNetworkStronghold successfully gets stronghold leaderboard with value 30", () => {
    apiMDRS.get.mockResolvedValue({ status: 200, data: successData })
    const expected = 30
    LeaderboardService.leaderboardNetworkStronghold()
        .then(res => expect(res.data[0].value).toBe(expected))
})