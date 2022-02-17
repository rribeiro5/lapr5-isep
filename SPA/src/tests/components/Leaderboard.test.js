import { act, render, screen } from '@testing-library/react'
import * as LeaderboardService from '../../services/LeaderboardService'
import Leaderboard from '../../components/Leaderboard/Leaderboard'
import '../../i18nextInit'

jest.mock('../../services/LeaderboardService')

const leaderboard = [
    { id: '1', name: "a", email: "a@gmail.com", avatar: undefined, value: 30 },
    { id: '2', name: "b", email: "b@gmail.com", avatar: undefined, value: 20 },
    { id: '3', name: "c", email: "c@gmail.com", avatar: undefined, value: 10 }
]

test("render dimension leaderboard div", async () => {
    LeaderboardService.leaderboardNetworkDimension.mockResolvedValue({ status: 200, data: leaderboard })
    LeaderboardService.leaderboardNetworkStronghold.mockResolvedValue({ status: 200, data: leaderboard })
    await act(async () => render(<Leaderboard nameCurrentComponent={()=>true} />))
    const element = screen.getByText('Network dimension')
    expect(element).toBeInTheDocument()
})

test("render stronghold leaderboard div", async () => {
    LeaderboardService.leaderboardNetworkDimension.mockResolvedValue({ status: 200, data: leaderboard })
    LeaderboardService.leaderboardNetworkStronghold.mockResolvedValue({ status: 200, data: leaderboard })
    await act(async () => render(<Leaderboard nameCurrentComponent={()=>true} />))
    const element = screen.getByText('Stronghold of the network')
    expect(element).toBeInTheDocument()
})

test("render dimension leaderboard positions", async () => {
    LeaderboardService.leaderboardNetworkDimension.mockResolvedValue({ status: 200, data: leaderboard })
    LeaderboardService.leaderboardNetworkStronghold.mockResolvedValue({ status: 200, data: leaderboard })
    await act(async () => render(<Leaderboard nameCurrentComponent={()=>true} />))
    const element = screen.getAllByRole('list')[0]
    expect(element.children.length).toEqual(3)
})

test("render stronghold leaderboard positions", async () => {
    LeaderboardService.leaderboardNetworkDimension.mockResolvedValue({ status: 200, data: leaderboard })
    LeaderboardService.leaderboardNetworkStronghold.mockResolvedValue({ status: 200, data: leaderboard })
    await act(async () => render(<Leaderboard nameCurrentComponent={()=>true} />))
    const element = screen.getAllByRole('list')[1]
    expect(element.children.length).toEqual(3)
})