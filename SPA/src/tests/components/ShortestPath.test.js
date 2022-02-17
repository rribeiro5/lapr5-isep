import React from "react";
import {act, fireEvent, render, screen} from '@testing-library/react';
import ShortestPath from '../../components/ShortestPath/ShortestPath'
import * as ApiAiService from "../../services/ApiAIService";
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
        "4@gmail.com",
        "2@gmail.com"
    ]
}


// Verify the shortest path was rendered
test("render shortest path  ", async () => {
    ApiAiService.getShortestTraject.mockResolvedValue({ status: 200, data: expectedResponse })
    await act( async () => render(<ShortestPath {...props}/>))
    const element=screen.getByText('2@gmail.com')
    expect(element).toBeInTheDocument()
})

// Verify the shortest path was rendered
test("render shortest path  ", async () => {
    ApiAiService.getShortestTraject.mockResolvedValue({ status: 200, data: expectedResponse })
    await act( async () => render(<ShortestPath {...props}/>))
    const element=screen.getByText('1@gmail.com')
    expect(element).toBeInTheDocument()
})

// Verify the shortest path was rendered
test("render shortest path  ", async () => {
    ApiAiService.getShortestTraject.mockResolvedValue({ status: 200, data: expectedResponse })
    await act( async () => render(<ShortestPath {...props}/>))
    const element=screen.getByText('4@gmail.com')
    expect(element).toBeInTheDocument()
})

