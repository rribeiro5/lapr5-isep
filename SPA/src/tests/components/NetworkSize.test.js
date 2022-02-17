import { act, render, screen, fireEvent } from '@testing-library/react'
import * as UserNetworkService from '../../services/UserNetworkService'
import NetworkSize from '../../components/NetworkSize/NetworkSize'
import '../../i18nextInit'

jest.mock('../../services/UserNetworkService')

const userId = '1'

const res = { value: 50 }

test("render button", () => {
    render(<NetworkSize userId={userId} />)
    const element = screen.getByRole("button")
    expect(element).toBeInTheDocument()
})

test("render level input", () => {
    render(<NetworkSize userId={userId} />)
    const btn = screen.getByRole("button")
    fireEvent.click(btn)
    const element = screen.getByRole("spinbutton")
    expect(element).toBeInTheDocument()
})

test("render submit button", () => {
    UserNetworkService.userNetworkSize.mockResolvedValue({ status: 200, data: res })
    render(<NetworkSize userId={userId} />)
    const btn = screen.getByRole("button")
    fireEvent.click(btn)
    const element = screen.getByText("Obtain Network")
    expect(element).toBeInTheDocument()
})