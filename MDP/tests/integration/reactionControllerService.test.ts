import * as sinon from 'sinon';

import { Response, Request, NextFunction } from 'express';

import { Result } from '../../src/core/logic/Result';

import ReactionController from '../../src/controllers/reactionController';
import IReactionService from '../../src/services/IServices/IReactionService';
import ReactionService from '../../src/services/reactionService';
import IPostRepo from '../../src/services/IRepos/IPostRepo'
import PostRepo from '../../src/repos/postRepo'
import ICreatingReactionResponseDTO from '../../src/dto/ICreatingReactionResponseDTO'

describe('reaction controller + service Integration Test', function () {
    const validRes = {
        publicationId: '123',
        publicationUserId: '3',
        userId: '1',
        incrementRelation: true,
        reaction: "LIKE"
    }
    
    beforeEach(function() {
    });

    it('createReactionPost: returns valid json', async function () {
        let body = { "publicationId": '123', "userId": '1', "reaction": "LIKE" }
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToPost").resolves(validRes);

        const service = new ReactionService(postRepo as IPostRepo);

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionPost(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(201));
        sinon.assert.calledOnce(res.json);
        sinon.assert.calledWith(res.json, sinon.match(validRes));
    })

    it('createReactionPost: returns bad request code for invalid json', async function () {
        let body = { "publicationId": '123', "userId": '1', "reaction": "FAIL" } // FAIL is not a reaction type
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const postRepo = new PostRepo(null);
        //sinon.stub(postRepo, "updateReactionToPost").resolves(null); // Not needed

        const service = new ReactionService(postRepo as IPostRepo);

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionPost(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    })

    it('createReactionPost: returns bad request when doesnt exist', async function () {
        let body = { "publicationId": '123', "userId": '123', "reaction": "LIKE" } // User 123 doesn't exist
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToPost").resolves(null);

        const service = new ReactionService(postRepo as IPostRepo);

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionPost(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    })

    it('createReactionCommentary: returns valid json', async function () {
        let body = { "publicationId": '123', "userId": '1', "reaction": "LIKE" }
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToComment").resolves(validRes);

        const service = new ReactionService(postRepo as IPostRepo);

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionCommentary(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(201));
        sinon.assert.calledOnce(res.json);
        sinon.assert.calledWith(res.json, sinon.match(validRes));
    })

    it('createReactionCommentary: returns bad request code for invalid json', async function () {
        let body = { "publicationId": '123', "userId": '1', "reaction": "FAIL" } // FAIL is not a reaction type
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const postRepo = new PostRepo(null);
        //sinon.stub(postRepo, "updateReactionToPost").resolves(null); // Not needed

        const service = new ReactionService(postRepo as IPostRepo);

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionCommentary(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    })

    it('createReactionCommentary: returns bad request when doesnt exist', async function () {
        let body = { "publicationId": '123', "userId": '123', "reaction": "LIKE" } // User 123 doesn't exist
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToComment").resolves(null);

        const service = new ReactionService(postRepo as IPostRepo);

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionCommentary(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    })
})