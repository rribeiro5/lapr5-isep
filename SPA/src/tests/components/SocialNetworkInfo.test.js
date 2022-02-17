import '../../i18nextInit'
import * as UserService from '../../services/UserService'
import {render, screen, fireEvent, act} from '@testing-library/react';
import * as ConnectionService from "../../services/ConnectionService";
import React from "react";
import SocialNetworkInfo from "../../components/SocialNetworkInfo/SocialNetworkInfo";


jest.mock('../../services/UserService')

const expected = {
    value: 2
}

test("Expect Size of Network is 2 ", async ()=>{
    UserService.getSocialNetworkDimension.mockResolvedValue({ status: 200, data: expected })
    await act(async () => render(
        <SocialNetworkInfo >
        </SocialNetworkInfo>
    ))
    const element = screen.getByText("2 users")
    expect(element).toBeInTheDocument()
})

test("Expect Size of Network is 0 ", async ()=>{
    UserService.getSocialNetworkDimension.mockResolvedValue({ status: 200, data: {value:0} })
    await act(async () => render(
        <SocialNetworkInfo >
        </SocialNetworkInfo>
    ))
    const element = screen.getByText("0 users")
    expect(element).toBeInTheDocument()
})