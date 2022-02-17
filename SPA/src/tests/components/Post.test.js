import {act, fireEvent, getByLabelText, render, screen} from '@testing-library/react';
import * as PostService from '../../services/PostService';
import Post from "../../components/Post/Post";
import CreatePostDTO from "../../model/Post/CreatePostDTO";

jest.mock('../../services/PostService')

jest.mock('react-i18next', () => ({
    // this mock makes sure any components using the translate hook can use it without a warning being shown
    useTranslation: () => {
        return {
            t: (str) => str,
            i18n: {
                changeLanguage: () => new Promise(() => {}),
            },
        };
    },
}));

const userId = "1234"
const posts = ["post 1", "post 2"]
const tags = ["tag1", "tag2"]

const result = {
    "id": "1",
    "userId": userId,
    "text": posts[0],
    "tags": tags,
    "comments": [],
    "reactions": [],
    "creationDateTime": 1640956638321
}

const dto = new CreatePostDTO(userId, posts[0], tags)

test("post updateTextArea is called once", async () => {
    const updateTextarea = jest.fn()
    await act(async () => render(
        <Post userId={userId} text={posts[0]} tags={tags} onChange={updateTextarea}/>
    ))
    expect(updateTextarea).calledOnce;
})

/*test("post updateTextArea changes value", async () => {
    // given
    const updateTextarea = jest.fn()
    await act(async () => render(
        <Post userId={userId} text='' tags={tags} onChange={updateTextarea}/>
    ))

    // when
    fireEvent.change(screen.getByRole('textbox'), { target: {
            name: 'text',
            value: posts[0]
        }});

    //then
    expect(updateTextarea).toHaveBeenCalledWith('text', posts[0]);
})

test("render post tags input", async () => {
    PostService.createPost.mockResolvedValue({ status: 201, data: result })
    const updateTextarea = jest.fn()
    await act(async () => render(
        <Post userId={userId} text={posts[0]} tags={tags} onChange={updateTextarea}/>
    ))
    const element = screen.getByText("tag1, tag2")
    expect(element).toBeInTheDocument()
})*/

/*
test("render post submit button", async () => {
    const updateTextarea = jest.fn()
    await act(async () => render(
        <Post userId={userId} text={posts[0]} tags={tags} onChange={updateTextarea}/>
    ))
    
    const element = screen.getByText("Submit")
    expect(element).toBeInTheDocument()
})

describe('Test Post Submit Button', () => {
    it('Test click event', () => {
        const mockCallBack = jest.fn();
        var btn = screen.getByRole('button');
        fireEvent.click(btn);
        expect(mockCallBack.mock.calls.length).toEqual(1);
    });
});*/
