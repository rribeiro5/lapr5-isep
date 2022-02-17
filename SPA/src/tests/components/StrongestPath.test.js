import {act, fireEvent, render, screen} from '@testing-library/react';
import StrongestPath from '../../components/StrongestPath/StrongestPath'
import * as ApiAiService from '../../services/ApiAIService'

import React from "react";
import * as UserService from "../../services/UserService";
import {getStrongestTrajectUnidirecional} from "../../services/ApiAIService";
import '../../i18nextInit'

jest.mock('../../services/ApiAIService')

const origUser = "1@gmail.com"
const destUser = "2@gmail.com"

const props = {
    origUser,destUser
}
const expectedResponse = {
    "caminho": [
        "1@gmail.com",
        "2@gmail.com"
    ],
    "forcaResultante": 9
}

// Verify the strongest unidirecional path button was rendered
test("render strongest unidirecional path button ", async () => {
    render(<StrongestPath {...props}/>)
    const element = screen.getAllByRole('button')[0]
    expect(element).toBeInTheDocument()
})

// Verify the strongest bidirecional path button was rendered
test("render strongest bidirecional path button ", async () => {
    render(<StrongestPath {...props}/>)
    const element = screen.getAllByRole('button')[1]
    expect(element).toBeInTheDocument()
})

// Verify the strongest unidirecional path was rendered
test("render strongest bidirecional path ", async () => {
    ApiAiService.getStrongestTrajectUnidirecional.mockResolvedValue({ status: 200, data: expectedResponse })
    render(<StrongestPath {...props}/>)
    const btn = screen.getAllByRole('button')[0]
    await act(async () => fireEvent.click(btn))
    const element=screen.getByText('2@gmail.com')
    expect(element).toBeInTheDocument()
})

// Verify the strongest Bidirecional path was rendered
test("render strongest bidirecional path ", async () => {
    ApiAiService.getStrongestTrajectBidirecional.mockResolvedValue({ status: 200, data: expectedResponse })
    render(<StrongestPath {...props}/>)
    const btn = screen.getAllByRole('button')[1]
    await act(async () => fireEvent.click(btn))
    const element=screen.getByText('1@gmail.com')
    expect(element).toBeInTheDocument()
})

