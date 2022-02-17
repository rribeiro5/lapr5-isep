
import * as PostService from '../../services/PostService'
import { act, render, screen, fireEvent } from '@testing-library/react'
import { ContextProvider } from '../../context/loggedUser'
import '../../i18nextInit'
import FeedPosts from "../../components/FeedPosts/FeedPosts";



jest.mock('../../services/PostService')

const context = {
    loggedUser:{
        id:"1"
    }
}

const user = {
    id:"1e37027b-03a9-4fd9-bd3b-e961d52c1d04"
}

const posts = [
    {
        "id": "1e37027b-03a9-4fd9-bd3b-e961d52c1d04",
        "userId": "9d3de456-8a2a-4254-b408-d5f48e65d64d",
        "text": "Teste de publicacao",
        "tags": [
            "tag1",
            "tag2"
        ],
        "comments": [],
        "reactions": [],
        "creationDateTime": 1640956638321
    }
]

test("Post with text 'Teste de publicacao' was rendered", async () => {
    PostService.feedPosts.mockResolvedValue({ status: 200, data: posts })
    await act(async () => render(
        <ContextProvider value={context}>
            <FeedPosts user={user} />
        </ContextProvider>))
    const element = screen.getByText("Teste de publicacao")
    expect(element).toBeInTheDocument()
})

test("Post with tags 'Tag1 , Tag2' was rendered", async () => {
    PostService.feedPosts.mockResolvedValue({ status: 200, data: posts })
    await act(async () => render(
        <ContextProvider value={context}>
            <FeedPosts user={user} />
        </ContextProvider>))
    const element = screen.getByText("tag1, tag2")
    expect(element).toBeInTheDocument()
})

test("Post published at '13:17:18' was rendered", async () => {
    PostService.feedPosts.mockResolvedValue({ status: 200, data: posts })
    await act(async () => render(
        <ContextProvider value={context}>
            <FeedPosts user={user} />
        </ContextProvider>))
    const element = screen.getByText("13:17:18")
    expect(element).toBeInTheDocument()
})