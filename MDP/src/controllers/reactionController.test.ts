import * as sinon from 'sinon';

import { Response, Request, NextFunction } from 'express';

import { Result } from '../core/logic/Result';

import ReactionController from './reactionController';
import IReactionService from '../services/IServices/IReactionService';
import ReactionService from '../services/reactionService';
import ICreatingReactionResponseDTO from '../dto/ICreatingReactionResponseDTO'

describe('reaction controller', function () {
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

        const service = new ReactionService(null);
        sinon.stub(service, "createReactionPost").returns(Result.ok<ICreatingReactionResponseDTO>(validRes))

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionPost(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(201));
        sinon.assert.calledOnce(res.json);
        sinon.assert.calledWith(res.json, sinon.match(validRes));
    })

    it('createReactionPost: returns bad request code for invalid json', async function () {
        let body = { "publicationId": '', "userId": '1', "reaction": "LIKE" }
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const service = new ReactionService(null);
        sinon.stub(service, "createReactionPost").returns(Result.fail<ICreatingReactionResponseDTO>("Error"))

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

        const service = new ReactionService(null);
        sinon.stub(service, "createReactionCommentary").returns(Result.ok<ICreatingReactionResponseDTO>(validRes))

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionCommentary(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(201));
        sinon.assert.calledOnce(res.json);
        sinon.assert.calledWith(res.json, sinon.match(validRes));
    })

    it('createReactionCommentary: returns bad request code for invalid json', async function () {
        let body = { "publicationId": '', "userId": '1', "reaction": "LIKE" }
        let req: Partial<Request> = {};
        req.body = body;

        let res: Partial<Response> = {
            json: sinon.spy(),
            status: sinon.spy(),
        }
        let next: Partial<NextFunction> = () => {};

        const service = new ReactionService(null);
        sinon.stub(service, "createReactionCommentary").returns(Result.fail<ICreatingReactionResponseDTO>("Error"))

        const ctrl = new ReactionController(service as IReactionService);
        await ctrl.createReactionCommentary(<Request>req, <Response>res, <NextFunction>next);

        sinon.assert.calledOnce(res.status);
        sinon.assert.calledWith(res.status, sinon.match(400));
    })
})