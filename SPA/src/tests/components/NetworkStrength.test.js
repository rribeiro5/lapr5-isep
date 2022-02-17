import { act, render, screen } from '@testing-library/react'
import * as UserNetworkService from '../../services/UserNetworkService'
import NetworkStrength from '../../components/NetworkStrength/NetworkStrength'
import '../../i18nextInit'

jest.mock('../../services/UserNetworkService')

const userId = '1'

const res = { value: 50 }

test("render element with value", async () => {
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: res })
    await act(async () => render(<NetworkStrength userId={userId} />))
    const element = screen.getByText(`Network Stronghold: ${res.value}`)
    expect(element).toBeInTheDocument()
})