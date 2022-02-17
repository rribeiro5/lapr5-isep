import * as ConnectionRequestService from "../../services/ConnectionRequestService";
import {act, fireEvent, render, screen} from "@testing-library/react";
import userEvent from '@testing-library/user-event'
import {ContextProvider} from "../../context/loggedUser";
import React from "react";
import TagCloud from "../../components/TagCloud/TagCloud"
import '../../i18nextInit'

jest.mock('../../services/TagCloudService')

import * as TagCloudService from "../../services/TagCloudService"
import {tagCloudAllConns, tagCloudOfUser, tagCloudOfUserConns} from "../../services/TagCloudService";

const ALL_USERS = "all-users"
const ALL_CONNS = "all-conns"
const LOGGED_USER = "logged-user"
const LOGGED_CONNS = "logged-conns"

const context = {
    loggedUser:{
        id:"1"
    }
}

const tags = [
    { value: 'JavaScript', count: 38 },
    { value: 'React', count: 30 },
    { value: 'Nodejs', count: 28 },
    { value: 'Express.js', count: 25 },
    { value: 'HTML5', count: 33 },
    { value: 'MongoDB', count: 18 },
    { value: 'CSS3', count: 20 },
]

test("Test render component with select",async ()=>{
    
    await act( async () => render(
        <ContextProvider value={context}>
            <TagCloud nameCurrentComponent={()=>true} />
        </ContextProvider>
    ))
    const element = screen.getByRole('combobox', { className:"tagcloud-type" })
    expect(element).toBeInTheDocument()
})


test("Tag cloud of all Users option selected",async ()=>{
    TagCloudService.tagCloudAllUsers.mockResolvedValue({ status: 200, data: tags })
    await act( async () => render(
        <ContextProvider value={context}>
            <TagCloud nameCurrentComponent={()=>true} />
        </ContextProvider>
    ))
    const element = screen.getByRole('combobox', { className:"tagcloud-type" })
    expect(element).toBeInTheDocument()
    
    await act(async () =>  userEvent.selectOptions(element,ALL_USERS))
    const selectedOption =  screen.getByRole('option',{name: 'All users'})
    expect(selectedOption).toBeInTheDocument()
    expect(screen.getByRole('option',{name: 'All users'}).selected).toBe(true)
    
    const tagCloudDiv = await screen.getByRole('tags')
    expect(tagCloudDiv).toBeInTheDocument()
    
    //const spanJavascript =  screen.getByText("JavaScript")
    //expect(spanJavascript).toBeInTheDocument()
    
   // const mongo = screen.getByText("MongoDB")
    //expect(mongo).toBeInTheDocument()
})


test("Tag Cloud of the All Connections Option selected",async ()=>{
    TagCloudService.tagCloudAllConns.mockResolvedValue({ status: 200, data: tags })
    await act( async () => render(
        <ContextProvider value={context}>
            <TagCloud nameCurrentComponent={()=>true} />
        </ContextProvider>
    ))
    const element = screen.getByRole('combobox', { className:"tagcloud-type" })
    expect(element).toBeInTheDocument()

    await act(async () =>  userEvent.selectOptions(element,ALL_CONNS))

    const selectedOption =  screen.getByRole('option',{name: 'All connections'})
    expect(selectedOption).toBeInTheDocument()
    expect(screen.getByRole('option',{name: 'All connections'}).selected).toBe(true)

    const tagCloudDiv = await screen.getByRole('tags')
    expect(tagCloudDiv).toBeInTheDocument()

    //const spanHTML5 =  screen.getByText("HTML5")
    //expect(spanHTML5).toBeInTheDocument()

    //const mongo = screen.getByText("MongoDB")
    //expect(mongo).toBeInTheDocument()
})


test("Tag Cloud of the User Option selected",async ()=>{
    TagCloudService.tagCloudOfUser.mockResolvedValue({ status: 200, data: tags })
    await act( async () => render(
        <ContextProvider value={context}>
            <TagCloud nameCurrentComponent={()=>true} />
        </ContextProvider>
    ))
    const element = screen.getByRole('combobox', { className:"tagcloud-type" })
    expect(element).toBeInTheDocument()

    await act(async () =>  userEvent.selectOptions(element,LOGGED_USER))
    
    const selectedOption =  screen.getByRole('option',{name: 'Logged user tags'})
    expect(selectedOption).toBeInTheDocument()
    expect(screen.getByRole('option',{name: 'Logged user tags'}).selected).toBe(true)

    const tagCloudDiv = await screen.getByRole('tags')
    expect(tagCloudDiv).toBeInTheDocument()

    //const spanNodejs =  screen.getByText("Nodejs")
    //expect(spanNodejs).toBeInTheDocument()

    //const CSS3 = screen.getByText("CSS3")
    //expect(CSS3).toBeInTheDocument()
})

test("Tag Cloud of the Connections of Logged User Option selected",async ()=>{
    TagCloudService.tagCloudOfUserConns.mockResolvedValue({ status: 200, data: tags })
    await act( async () => render(
        <ContextProvider value={context}>
            <TagCloud nameCurrentComponent={()=>true} />
        </ContextProvider>
    ))
    const element = screen.getByRole('combobox', { className:"tagcloud-type" })
    expect(element).toBeInTheDocument()

    await act(async () =>  userEvent.selectOptions(element,LOGGED_CONNS))

    const selectedOption =  screen.getByRole('option',{name: 'Logged user connection tags'})
    expect(selectedOption).toBeInTheDocument()
    expect(screen.getByRole('option',{name: 'Logged user connection tags'}).selected).toBe(true)

    const tagCloudDiv = await screen.getByRole('tags')
    expect(tagCloudDiv).toBeInTheDocument()

    //const spanHTML5 =  screen.getByText("HTML5")
    //expect(spanHTML5).toBeInTheDocument()

})