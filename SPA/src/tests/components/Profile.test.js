import { act, render, screen,fireEvent } from '@testing-library/react'
import Profile from '../../components/Profile/Profile'
import * as UserService from '../../services/UserService'
import * as UserNetworkService from '../../services/UserNetworkService'
import * as PostService from '../../services/PostService'
import PrivateProfileDTO from '../../model/Profile/PrivateProfileDTO'
import {ContextProvider} from "../../context/loggedUser";
import IntroductionConnectionRequest from "../../components/IndirectConnectionRequest/IntroductionConnectionRequest";
import React from "react";
import '../../i18nextInit'

jest.mock('../../services/UserService')
jest.mock('../../services/UserNetworkService')
jest.mock('../../services/PostService')

const id = "1"
const avatar = "avatar.png"
const name = "name"
const email = "email"
const phoneNumber = "phoneNumber"
const birthdayDate = "birthdayDate"
const city = "city"
const country = "Portugal"
const description = "description"
const points = "points"
const linkedInURL = "linkedInUrl"
const facebookURL = "facebookUrl"
const interestTags = ["interestTags"]
const emotionalState = "neutral"
/*
const profileDTO=new PrivateProfileDTO(
    id,avatar,name,email,phoneNumber,
    birthdayDate,city,country,description,points,
    linkedInURL, facebookURL,interestTags,emotionalState)
*/

const profile={
    "id" : id,
    "avatar" : avatar,
    "name" : name,
    "email" : email,
    "phoneNumber" : phoneNumber,
    "birthdayDate" : birthdayDate,
    "city" : city,
    "country" : country,
    "description" : description,
    "points" : points,
    "linkedInURL" : linkedInURL,
    "facebookURL" : facebookURL,
    "interestTags" : interestTags,
    "emotionalState" : emotionalState
}

const networkStronghold = { value: 100 }

const context = {
    loggedUser:{
        id:"1"
    }
}
//get emotional state
// Verify the select element was rendered
test("render emotional state",async ()=>{
    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button')[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Emotional State')
    expect(element).toBeInTheDocument()
})


test("confirm change emotional state",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button')[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Emotional State')
    element.setAttribute("value","changed")
    expect(element).toHaveAttribute("value","changed")
})


test("render name",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "popup-button" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Name')
    expect(element).toBeInTheDocument()
})


test("confirm change name",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Name')
    element.setAttribute("value","test")
    expect(element).toHaveAttribute("value","test")
})

test("render description",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Profile Description')
    expect(element).toBeInTheDocument()
})


test("confirm change description",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Profile Description')
    element.setAttribute("value","New Description")
    expect(element).toHaveAttribute("value","New Description")
})

test("render phone number",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Phone Number')
    expect(element).toBeInTheDocument()
})


test("confirm change phone number",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Phone Number')
    element.setAttribute("value","+3511111")
    expect(element).toHaveAttribute("value","+3511111")
})

test("render linkedin",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Linkedin')
    expect(element).toBeInTheDocument()
})


test("confirm change linkedin",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Linkedin')
    element.setAttribute("value","https://www.linkedin.com/in/Teste1")
    expect(element).toHaveAttribute("value","https://www.linkedin.com/in/Teste1")
})

test("render facebook",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Facebook')
    expect(element).toBeInTheDocument()
})


test("confirm change facebook",async ()=>{

    UserService.getPrivateProfile.mockResolvedValue({ status: 200, data: profile })
    UserNetworkService.userNetworkStrength.mockResolvedValue({ status: 200, data: networkStronghold })
    PostService.feedPosts.mockResolvedValue({ status: 200, data: [] })
    await act( async () => render(
        <ContextProvider value={context}>
            <Profile nameCurrentComponent={() => true}/>
        </ContextProvider>   )
    )
    const btn = screen.getAllByRole('button', { className: "buttons" })[0]
    fireEvent.click(btn)
    const element=screen.getByPlaceholderText('Facebook')
    element.setAttribute("value","https://www.facebook.com/TheBest1")
    expect(element).toHaveAttribute("value","https://www.facebook.com/TheBest1")
})