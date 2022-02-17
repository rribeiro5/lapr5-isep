import * as PostService from '../../services/PostService'
import apiMDRS from '../../services/apiMDRS'
import CreateCommenttDTO from "../../model/Post/CreateCommentDTO";
import CreatePostDTO from "../../model/Post/CreatePostDTO";

jest.mock('../../services/apiMDRS')

const postId="1"
const userId="2"
const text="sample"
const tags=["a","b"]
const comment=new CreateCommenttDTO(postId,userId,text)
const post = new CreatePostDTO(userId,text,tags)

test("create comment successfully returns 201 status code",()=>{
    apiMDRS.post.mockResolvedValue({ status: 201, data: "" })
    const expected=201
    PostService.createComment(comment)
        .then(res=>expect(res.status).toBe(expected))
})

test("create comment successfully returns expected data",()=>{
    apiMDRS.post.mockResolvedValue({ status: 201, data: comment })
    const expectedPostId=comment.postId
    const expectedUserId=comment.userId
    const expectedText=comment.text
    PostService.createComment(comment)
        .then(res=>expect(res.data.postId).toBe(expectedPostId)
            && expect(res.data.userId).toBe(expectedUserId)
            && expect(res.data.text).toBe(expectedText))

})

test("fail to create comment with invalid data",()=>{
    apiMDRS.post.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected=404

    PostService.createComment(null)
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))

})

test("create post successfully returns 201 status code",()=>{
    apiMDRS.post.mockResolvedValue({ status: 201, data: "" })
    const expected=201
    PostService.createPost(post)
        .then(res=>expect(res.status).toBe(expected))
})

test("create post successfully returns expected data",()=>{
    apiMDRS.post.mockResolvedValue({ status: 201, data: post })
    const expectedTags=post.tags
    const expectedUserId=post.userId
    const expectedText=post.text
    PostService.createPost(post)
        .then(res=>expect(res.data.tags).toBe(expectedTags)
            && expect(res.data.userId).toBe(expectedUserId)
            && expect(res.data.text).toBe(expectedText))

})

test("fail to create post with invalid data",()=>{
    apiMDRS.post.mockImplementation(() => Promise.reject({ response: { status: 404, data: {} } }))
    const expected=404

    PostService.createPost(null)
        .then(() => fail('Request should fail'))
        .catch(err => expect(err.response.status).toBe(expected))

})