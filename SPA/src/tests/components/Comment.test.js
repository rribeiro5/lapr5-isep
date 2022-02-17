import { act, render, screen } from '@testing-library/react'
import * as PostService from '../../services/PostService'
import {ContextProvider} from "../../context/loggedUser"
import '../../i18nextInit'
import Comment from "../../components/Comment/Comment";
import CreateCommentDTO from "../../model/Post/CreateCommentDTO";

jest.mock('../../services/PostService')

const context = {
    loggedUser:{
        id:"1"
    }
}

const postId = "123"

const comments = ["comment 1", "comment 2"]
const dto=new CreateCommentDTO(postId,context.loggedUser.id,comments[0],[])


test("render comment textarea", async () => {
    await act(async () => render(
        <ContextProvider value={context}>
            <Comment postId={postId} />
        </ContextProvider>
    ))
    const element = screen.getByPlaceholderText("Add a comment...")
    expect(element).toBeInTheDocument()
})


test("render comment submit button", async () => {
    //PostService.createComment.mockResolvedValue({ status: 200, data: {} })
    const val=PostService.createComment(dto);
    await act(async () => render(
        <ContextProvider value={context}>
            <Comment postId={postId} />
        </ContextProvider>
    ))
    const element = screen.getByText("Submit")
    expect(element).toBeInTheDocument()
})
