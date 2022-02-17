import { act, render, screen } from '@testing-library/react'
import Reaction from '../../components/Reaction/Reaction'
import * as PostService from '../../services/PostService'
import {ContextProvider} from "../../context/loggedUser"
import '../../i18nextInit'

jest.mock('../../services/PostService')

const context = {
    loggedUser:{
        id:"1"
    }
}

const publicationId = "123"

const reactions = [{ reaction: "LIKE" }, { reaction: "DISLIKE" }, { reaction: "LIKE" }]

test("render correct number of likes", async () => {
    PostService.updateReactionPost.mockResolvedValue({ status: 200, data: {} })
    await act(async () => render(
        <ContextProvider value={context}>
            <Reaction publicationId={publicationId} reactions={reactions} updateFeed={() => true} />
        </ContextProvider>
    ))
    const element = screen.getByText("2")
    expect(element).toBeInTheDocument()
})

test("render like button", async () => {
    PostService.updateReactionPost.mockResolvedValue({ status: 200, data: {} })
    await act(async () => render(
        <ContextProvider value={context}>
            <Reaction publicationId={publicationId} reactions={reactions} updateFeed={() => true} />
        </ContextProvider>
    ))
    const element = screen.getByText("2")
    expect(element.parentElement.firstChild).toBeInTheDocument()
})

test("render correct number of dislikes", async () => {
    PostService.updateReactionPost.mockResolvedValue({ status: 200, data: {} })
    await act(async () => render(
        <ContextProvider value={context}>
            <Reaction publicationId={publicationId} reactions={reactions} updateFeed={() => true} />
        </ContextProvider>
    ))
    const element = screen.getByText("1")
    expect(element).toBeInTheDocument()
})

test("render dislike button", async () => {
    PostService.updateReactionPost.mockResolvedValue({ status: 200, data: {} })
    await act(async () => render(
        <ContextProvider value={context}>
            <Reaction publicationId={publicationId} reactions={reactions} updateFeed={() => true} />
        </ContextProvider>
    ))
    const element = screen.getByText("1")
    expect(element.parentElement.firstChild).toBeInTheDocument()
})