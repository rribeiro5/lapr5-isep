import { act, render, screen,fireEvent } from '@testing-library/react'
import * as UserService from '../../services/UserService'
import DirectRequest from "../../components/DirectRequest/DirectRequest";
import {Context, ContextProvider} from "../../context/loggedUser";
import DirectConnectionRequest from "../../components/DirectConnectionRequest/DirectConnectionRequest";
import React, {useContext} from "react";
import '../../i18nextInit'

jest.mock('../../services/UserService')

const loggedUser={"id":'1'}

const users=[]

const context = {
    loggedUser:{
        id:"1"
    }
}

test("render name Option",async ()=>{
    UserService.getUsersByName.mockResolvedValue({ status: 200, data: users })
    
    await act(async () => render(
        <ContextProvider value={context}>
            <DirectRequest nameCurrentComponent={()=>true} />))
        </ContextProvider>))
  
    
    const element = screen.getByDisplayValue('Name')
    expect(element).toBeInTheDocument()
})

test("render Country Option",async ()=>{
    UserService.getUsersByName.mockResolvedValue({ status: 200, data: users })
    await act(async () => render(
        <ContextProvider value={context}>
            <DirectRequest nameCurrentComponent={()=>true} />))
        </ContextProvider>))
    const element = screen.getByText('Country')
    expect(element).toBeInTheDocument()
})
