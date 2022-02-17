
import * as sinon from 'sinon';


import { Response, Request, NextFunction } from 'express';

import { assert } from 'console';
import PostRepo from '../../src/repos/postRepo';
import IPostRepo from '../../src/services/IRepos/IPostRepo';
import IPostController from '../../src/controllers/IControllers/IPostController';
import postController from '../../src/controllers/postController';
import IPostService from '../../src/services/IServices/IPostService';
import PostService from '../../src/services/postService';
import { Result } from '../../src/core/logic/Result';
import ICreatingCommentResponseDTO from '../../src/dto/ICreatingCommentResponseDTO';
import { Post } from '../../src/domain/post';

describe('post controller + service Integration Test', function () {

    let postRepo: IPostRepo
    let service: IPostService
    let ctrl: IPostController

    beforeEach(function () {
        postRepo = new PostRepo(null);
        service = new PostService(postRepo);
        ctrl = new postController(service);
    });

    afterEach(function () {
        sinon.restore();
    });

    const commentData = {
        userId: "1234",
        text: "text",
        reactions: [],

        expected: {
            userId: "1234",
            text: "olá, boa tarde",
            reactions: [],
        },

        body: {
            postId: "1",
            userId: "1234",
            text: "olá, boa tarde",
            reactions: []
        },
        createPost: () => { return Post.create(commentData.body).getValue() },
        resetCommentBodyText: () => { commentData.body.text = "olá, boa tarde" }
    }

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
      userId: "1234",
      text: "olá, boa tarde",
      tags: [],
    },
    createPost: () => { return Post.create(postData.body).getValue() },
    resetPostBodyText: () => { postData.body.text = "olá, boa tarde" }
  }

    it('Comment: returns 201 for valid parameters', async function () {
        let req: Partial<Request> = {}
        req.body = commentData.body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { };
        const post = commentData.createPost()
        sinon.stub(postRepo, "updateCommentToPost").returns(post)
        await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(201));
    });

    it('Comment: returns expected dto for valid parameters', async function () {
        let req: Partial<Request> = {}
        req.body = commentData.body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { };
        const post = commentData.createPost()
        sinon.stub(postRepo, "updateCommentToPost").returns(post)
        await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        sinon.assert.calledOnce(res.json);
        sinon.assert.calledWith(res.json, sinon.match(commentData.expected));
    });

    it('Comment: returns bad request for invalid parameters', async function () {
        let req: Partial<Request> = {}
        req.body = commentData.body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { };
        commentData.body.text = ""
        sinon.stub(postRepo, "updateCommentToPost").returns(commentData.body)
        await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    });

    it('Comment: returns next function when exception is thrown', async function () {
        let req: Partial<Request> = {}
        req.body = commentData.body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { return true };
        commentData.body.text = ""

        sinon.stub(postRepo, "updateCommentToPost").throws();
        const result = await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        assert(result)
    });

  it('Post: returns 201 for valid parameters', async function () {
    let req: Partial<Request> = {}
    req.body = postData.body;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }

    let next: Partial<NextFunction> = () => { };
    const post = postData.expected
    sinon.stub(postRepo, "save").returns(post)
    await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    sinon.assert.calledOnce(res.status);
    sinon.assert.calledWith(res.status, sinon.match(201));
  });

/*  it('Post: returns expected dto for valid parameters', async function () {
    let req: Partial<Request> = {}
    req.body = postData.body;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }
    let next: Partial<NextFunction> = () => { };

    const post = postData.expected
    sinon.stub(postRepo, "save").returns(post)
    await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    sinon.assert.calledOnce(res.json);
    sinon.assert.calledWith(res.json, sinon.match(postData.expected));
  });*/

  it('Post: returns bad request for invalid parameters', async function () {
    let req: Partial<Request> = {}
    req.body = postData.body;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }

    let next: Partial<NextFunction> = () => { };
    postData.body.text = ""
    sinon.stub(postRepo, "save").returns(postData.body)
    await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    sinon.assert.calledOnce(res.status);
    sinon.assert.calledWith(res.status, sinon.match(402));
  });

  it('Post: returns next function when exception is thrown', async function () {
    let req: Partial<Request> = {}
    req.body = postData.body;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }

    let next: Partial<NextFunction> = () => { return true };
    postData.body.text = ""

    sinon.stub(postRepo, "save").throws();
    const result = await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    assert(result)
  });
});
