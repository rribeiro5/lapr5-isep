
import * as sinon from 'sinon';


import { Response, Request, NextFunction } from 'express';

import { Container } from 'typedi';
import config from "../../config";

import { Result } from '../core/logic/Result';
import mongooseLoader from '../loaders/mongoose';
import IPostService from "../services/IServices/IPostService";
import PostService from '../services/postService';
import postController from "./postController";
import IPostDTO from '../dto/IPostDTO';
import dependencyInjectorLoader from '../loaders/dependencyInjector';
import ICreatingCommentDTO from '../dto/ICreatingCommentDTO';
import IReactionDTO from '../dto/IReactionDTO';
import ICreatingCommentResponseDTO from '../dto/ICreatingCommentResponseDTO';
import { assert } from 'console';
import {Comment} from "../domain/Comment/Comment";
import {Tag} from "../domain/tag";
import ICommentDTO from "../dto/ICommentDTO";


describe('post controller', function () {
	const expectedPosts : IPostDTO[] = [
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

    beforeEach(function() {
    })


    it('feedPosts: returns json with array with post of user values', async function () {

        let userId = "1e37027b-03a9-4fd9-bd3b-e961d52c1d04";
        let req: Partial<Request> = {};
        let body = {}
        req.body = body;
        req.params={userId:""}
		req.params.userId = userId;

        let res: Partial<Response> = {
			status: sinon.spy(),
            json: sinon.spy()
        };

		let next: Partial<NextFunction> = () => {};

        const service : IPostService= new PostService(null);
        sinon.stub(service,"feedPosts").returns(Result.ok<IPostDTO[]>(expectedPosts));

		const ctrl = new postController(service as IPostService);
		await ctrl.feedPosts(<Request>req, <Response>res, <NextFunction>next);

		sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status,sinon.match(200));
        sinon.assert.calledOnce(res.json);
		sinon.assert.calledWith(res.json, sinon.match(expectedPosts));
	});


    it('feedPosts: returns bad request for invalid user id', async function () {


        let req: Partial<Request> = {};
        let body = {}
        req.body = body;
        req.params={userId:""}


        let res: Partial<Response> = {
			status: sinon.spy(),
            json: sinon.spy()
        };

		let next: Partial<NextFunction> = () => {};

        const service : IPostService= new PostService(null);
        sinon.stub(service,"feedPosts").returns(Result.fail<IPostDTO[]>("Error"));

		const ctrl = new postController(service as IPostService);
		await ctrl.feedPosts(<Request>req, <Response>res, <NextFunction>next);

		sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status,sinon.match(400));
	});

    /*
    ----comment
    */
    let userId: string = "1234"
    let text: string = "text"
    let reactions: IReactionDTO[] = []


    let validRes: ICreatingCommentResponseDTO = {
        id:"1",
        postId: "1",
        userId: userId,
        text: text,
        reactions: reactions,
        creationDateTime: 10,
    }

    let body: ICreatingCommentDTO = {
        postId: "1",
        userId: userId,
        text: text,
        reactions: reactions
    }

    it('Comment: returns 201 for valid parameters',async function(){
        let req: Partial<Request> = {}
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { };
        let service = new PostService(null);
        let ctrl = new postController(service as IPostService);
        sinon.stub(service, "createCommentPost").returns(Result.ok<ICreatingCommentResponseDTO>(validRes))
        await ctrl.createCommentPost(<Request>req,<Response>res,<NextFunction>next)
        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(201));
    });

    it('Comment: returns expected dto for valid parameters', async function () {
        let req: Partial<Request> = {}
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { };
        let service = new PostService(null);
        let ctrl = new postController(service as IPostService);
        sinon.stub(service, "createCommentPost").returns(Result.ok<ICreatingCommentResponseDTO>(validRes))
        await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        sinon.assert.calledOnce(res.json);
        sinon.assert.calledWith(res.json, sinon.match(validRes));
    });

    it('Comment: returns bad request for invalid parameters', async function () {
        let req: Partial<Request> = {}
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }

        let next: Partial<NextFunction> = () => { };
        let service = new PostService(null);
        let ctrl = new postController(service as IPostService);
        body.text = ""
        sinon.stub(service, "createCommentPost").returns(Result.fail<ICreatingCommentResponseDTO>(validRes))
        await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    });

    it('Comment: returns next function when exception is thrown', async function () {
        let req: Partial<Request> = {}
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {return true;};
        let service = new PostService(null);
        let ctrl = new postController(service as IPostService);
        sinon.stub(service, "createCommentPost").throws();
        const result=await ctrl.createCommentPost(<Request>req, <Response>res, <NextFunction>next)
        assert(result)
        sinon.restore();
    });

    /*
      ----Post
    */

    let tags: string [] = []
    let comments: ICommentDTO [] = []

    let validPost: IPostDTO = {
      id:"1",
      userId: userId,
      text: text,
      reactions: reactions,
      tags: tags,
      comments: comments,
      creationDateTime: 10,
    }

    let postBody: IPostDTO = {
      id:"",
      userId: userId,
      text: text,
      reactions: reactions,
      tags: tags,
      comments: comments,
      creationDateTime: 0,
    }

  it('Post: returns 201 for valid parameters',async function(){
    let req: Partial<Request> = {}
    req.body = postBody;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }

    let next: Partial<NextFunction> = () => { };
    let service = new PostService(null);
    let ctrl = new postController(service as IPostService);
    sinon.stub(service, "createPost").returns(Result.ok<IPostDTO>(validPost))
    await ctrl.createPost(<Request>req,<Response>res,<NextFunction>next)
    sinon.assert.calledOnce(res.status);
    sinon.assert.calledWith(res.status, sinon.match(201));
  });

  it('Post: returns expected dto for valid parameters', async function () {
    let req: Partial<Request> = {}
    req.body = postBody;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }

    let next: Partial<NextFunction> = () => { };
    let service = new PostService(null);
    let ctrl = new postController(service as IPostService);
    sinon.stub(service, "createPost").returns(Result.ok<IPostDTO>(validPost))
    await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    sinon.assert.calledOnce(res.json);
    sinon.assert.calledWith(res.json, sinon.match(validPost));
  });

  it('Post: returns bad request for invalid parameters', async function () {
    let req: Partial<Request> = {}
    req.body = postBody;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }

    let next: Partial<NextFunction> = () => { };
    let service = new PostService(null);
    let ctrl = new postController(service as IPostService);
    postBody.text = ""
    sinon.stub(service, "createPost").returns(Result.fail<IPostDTO>(validPost))
    await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    sinon.assert.calledOnce(res.status);
    sinon.assert.calledWith(res.status, sinon.match(402));
  });

  it('Post: returns next function when exception is thrown', async function () {
    let req: Partial<Request> = {}
    req.body = postBody;

    let res: Partial<Response> = {
      json: sinon.spy(),
      status: sinon.spy(),
    }
    let next: Partial<NextFunction> = () => {return true;};
    let service = new PostService(null);
    let ctrl = new postController(service as IPostService);
    sinon.stub(service, "createPost").throws();
    const result=await ctrl.createPost(<Request>req, <Response>res, <NextFunction>next)
    assert(result)
    sinon.restore();
  });
});
