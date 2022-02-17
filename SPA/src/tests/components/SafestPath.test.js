import {act, fireEvent, render, screen} from '@testing-library/react';
import SafestPath from '../../components/SafestPath/SafestPath'
import * as ApiAiService from '../../services/ApiAIService'
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

// Verify the safest unidirecional path button was rendered
test("render safest unidirecional path button ", async () => {
    render(<SafestPath {...props}/>)
    const element = screen.getAllByRole('button')[0]
    expect(element).toBeInTheDocument()
})

// Verify the safest bidirecional path button was rendered
test("render safest bidirecional path button ", async () => {
    render(<SafestPath {...props}/>)
    const element = screen.getAllByRole('button')[1]
    expect(element).toBeInTheDocument()
})

// Verify the minimum strength input was rendered
test("render minimum strength input", async () => {
    render(<SafestPath {...props}/>)
    const element = screen.getByPlaceholderText("Minime Value for Connection")
    expect(element).toBeInTheDocument()
})