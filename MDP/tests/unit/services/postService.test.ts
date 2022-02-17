import { expect } from 'chai';
import PostService from "../../../src/services/postService";
import IPostService from '../../../src/services/IServices/IPostService';
import IPostDTO  from '../../../src/dto/IPostDTO';
import  {Post} from "../../../src/domain/post";
import PostRepo from "../../../src/repos/postRepo";
import * as sinon from 'sinon';
import { UniqueEntityID } from '../../../src/core/domain/UniqueEntityID';
import { assert } from 'console';
import IPostRepo from '../../../src/services/IRepos/IPostRepo';

describe('Test Post Service', () => {



  let postRepo: IPostRepo
  let service: IPostService

  beforeEach(function () {
    postRepo = new PostRepo(null);
    service = new PostService(postRepo);
  });

  afterEach(function () {
    sinon.restore();
  });

  const feedPostData = {
    userId: "1234",
    expected: {
      id: "1",
      userId: "1234",
      reactions : [],
      comments : [],
      text: "olá, boa tarde",
      tags: [],
      creationDateTime: 10,
    }
  }



  it('Expect success response of feed Posts with valid user', async () => {
    const listPosts : Post[]   = [commentData.createPost()]
    sinon.stub(postRepo, "findAllByUserId").returns(listPosts)
    const result = await service.feedPosts(feedPostData.userId)
    assert(result.isSuccess)
  });

  it('Expect failure response of feed Posts with invalid user id', async () => {
    const invalidUserId = "";
    const result = await service.feedPosts(invalidUserId)
    assert(result.isFailure)
  });

  it('Expect failure response of feed Posts with invalid posts', async () => {
    const invalidListPosts : Post[]   = null
    sinon.stub(postRepo, "findAllByUserId").returns(invalidListPosts)
    const result = await service.feedPosts(feedPostData.userId)
    assert(result.isFailure)
  });

  it('Expect correct dto with valid feed post for valid user', async function () {
    const listPosts : Post[]   = [commentData.createPost()]
    sinon.stub(postRepo, "findAllByUserId").returns(listPosts)

    const result = await service.feedPosts(feedPostData.userId)

    const expected=commentData.expected;
    assert(result.isSuccess)
    assert(expected === result[0]);
  })

  /*
  Comment
  */

  const commentData = {
    userId: "1234",
    text: "text",
    reactions: [],

    expected: {
        id: "1",
        postId: "1",
        userId: "1234",
        text: "olá, boa tarde",
        reactions: [],
        creationDateTime: 10,

    },
    body: {
      postId: "1",
      userId: "1234",
      text: "olá, boa tarde",
      reactions: []
    },
    createPost: () => { return Post.create(commentData.body).getValue()},
    resetCommentBodyText :() => { commentData.body.text = "olá, boa tarde" }
  }

  it('Expect success with valid comment data',async function(){
    const post = commentData.createPost()
    sinon.stub(postRepo, "updateCommentToPost").returns(post)
    const result=await service.createCommentPost(commentData.body)
    assert(result.isSuccess)
  })

  it('Expect correct dto with valid comment data', async function () {
    const post = commentData.createPost()
    sinon.stub(postRepo, "updateCommentToPost").returns(post)
    const result = await service.createCommentPost(commentData.body)
    const expected=commentData.expected;
    assert(expected.postId === result.getValue().postId);
    assert(expected.userId === result.getValue().userId);
    assert(expected.text === result.getValue().text);
    assert(expected.reactions === result.getValue().reactions);
  })


  it('Expect failure when addCommentToPost is null', async function () {
    sinon.stub(postRepo, "updateCommentToPost").returns(null)
    const result = await service.createCommentPost(commentData.body)
    assert(result.isFailure)
  })

  /*
      Post
   */

  const postData = {
    userId: "1234",
    text: "text",
    tags: [],
    reactions: [],
    comments: [],
    creationDateTime: 12,

    expected: {
      id: "1",
      userId: "1234",
      text: "olá, boa tarde",
      tags: [],
      reactions: [],
      comments: [],
      creationDateTime: 12
    },

    body: {
      id:"",
      userId: "1234",
      text: "olá, boa tarde",
      tags: [],
      reactions: [],
      comments: [],
      creationDateTime: 12
    },
    createPost: () => { return Post.create(postData.body).getValue() },
    resetPostBodyText: () => { postData.body.text = "olá, boa tarde" }
  }

  it('Expect success with valid post data',async function(){
    const post = postData.createPost()
    sinon.stub(postRepo, "save").returns(post)
    const result=await service.createPost(postData.body)
    assert(result.isSuccess)
  })

  it('Expect correct dto with valid post data', async function () {
    const post = postData.createPost()
    sinon.stub(postRepo, "save").returns(post)
    const result = await service.createPost(postData.body)
    const expected=postData.expected;
    assert(expected.id === result.getValue().id);
    assert(expected.userId === result.getValue().userId);
    assert(expected.text === result.getValue().text);
    assert(expected.reactions === result.getValue().reactions);
  })


  it('Expect failure when save is null', async function () {
    sinon.stub(postRepo, "save").returns(null)
    const result = await service.createPost(postData.body)
    assert(result.isFailure)
  })


});

