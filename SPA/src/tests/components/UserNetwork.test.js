import {render, screen, fireEvent, act} from '@testing-library/react';
import UserNetwork from '../../components/UserNetwork/UserNetwork'
import * as UserService from '../../services/UserService'
import {ContextProvider} from "../../context/loggedUser";
import UpdateConnection from "../../components/UpdateConnection/UpdateConnection";
import React from "react";
import '../../i18nextInit'

jest.mock('../../services/UserService')

const context = {
    loggedUser:{
        id:"1"
    }
}

const expected = {
    "userLevel": 0,
    "id": "85e23203-e175-41b7-aab9-78a9521dfad6",
    "email": "1@gmail.com",
    "interestTags": [
        "tag1",
        "tag2"
    ],
    "emotionalState": null,
    "connections": [
        {
            "connectionStrength": 4,
            "relationshipStrength": 0,
            "connTags": [
                "ABC",
                "DEF"
            ],
            "user": {
                "userLevel": 1,
                "id": "8cbce14b-e9e2-4ae4-98ed-0d79eb7e55db",
                "email": "2@gmail.com",
                "interestTags": [
                    "tag1",
                    "tag2"
                ],
                "emotionalState": null,
                "connections": []
            }
        }
    ]
}

const currentComp = ()=>{
    
}


// Verify that the level input was rendered
test("render level input", () => {
    render(
        <ContextProvider value={context}>
            <UserNetwork nameCurrentComponent={currentComp} />
        </ContextProvider>
    )
    const element = screen.getByPlaceholderText("Level")
    expect(element).toBeInTheDocument()
})

// Verify that submit button was rendered after clicking button
test("render submit button", () => {
    render(
        <ContextProvider value={context}>
            <UserNetwork nameCurrentComponent={currentComp} />
        </ContextProvider>   
    )
    const element = screen.getByRole('button')
    expect(element).toBeInTheDocument()
})

test("render traject", async () => {
    UserService.getUserNetwork.mockResolvedValue({ status: 200, data: { expected } })
    render(
        <ContextProvider value={context}>
            <UserNetwork nameCurrentComponent={currentComp} />
        </ContextProvider>
    )
    const btn = screen.getAllByRole('button')[0]
    expect(btn).toBeInTheDocument()
})

