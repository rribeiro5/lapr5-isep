import { expect } from 'chai';
import * as sinon from 'sinon';

import { Result } from '../../../src/core/logic/Result';

import ReactionService from '../../../src/services/reactionService'
import IPostRepo from '../../../src/services/IRepos/IPostRepo'
import PostRepo from '../../../src/repos/postRepo'
import ICreatingReactionDTO from '../../../src/dto/ICreatingReactionDTO';

describe('reactionService Test', () => {
    const validRes = {
        publicationId: '123',
        publicationUserId: '3',
        userId: '1',
        incrementRelation: true,
        reaction: "LIKE"
    }
    
    it('createReactionPost: returns expected result', async () => {
        const reactionDto : ICreatingReactionDTO = { "publicationId": '123', "userId": '1', "reaction": "LIKE" }

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToPost").resolves(validRes);

        const service = new ReactionService(postRepo as IPostRepo);
        const res = await service.createReactionPost(reactionDto);

        expect(res.isSuccess).to.equal(true);
        expect(res.getValue()).to.equal(validRes);
    })

    it('createReactionPost: returns fail result when user doesnt exist', async () => {
        const reactionDto : ICreatingReactionDTO = { "publicationId": '123', "userId": '123', "reaction": "LIKE" } // User 123 doesn't exist

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToPost").resolves(null);

        const service = new ReactionService(postRepo as IPostRepo);
        const res = await service.createReactionPost(reactionDto);

        expect(res.isFailure).to.equal(true);
    })

    it('createReactionPost: returns fail result when using invalid data (reaction type)', async () => {
        const reactionDto : ICreatingReactionDTO = { "publicationId": '123', "userId": '1', "reaction": "FAIL" } // FAIL is not a reaction type

        const postRepo = new PostRepo(null);
        //sinon.stub(postRepo, "updateReactionToPost").resolves(null); // Not needed

        const service = new ReactionService(postRepo as IPostRepo);
        const res = await service.createReactionPost(reactionDto);

        expect(res.isFailure).to.equal(true);
    })

    it('createReactionCommentary: returns expected result', async () => {
        const reactionDto : ICreatingReactionDTO = { "publicationId": '123', "userId": '1', "reaction": "LIKE" }

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToComment").resolves(validRes);

        const service = new ReactionService(postRepo as IPostRepo);
        const res = await service.createReactionCommentary(reactionDto);

        expect(res.isSuccess).to.equal(true);
        expect(res.getValue()).to.equal(validRes);
    })

    it('createReactionCommentary: returns fail result when user doesnt exist', async () => {
        const reactionDto : ICreatingReactionDTO = { "publicationId": '123', "userId": '123', "reaction": "LIKE" } // User 123 doesn't exist

        const postRepo = new PostRepo(null);
        sinon.stub(postRepo, "updateReactionToComment").resolves(null);

        const service = new ReactionService(postRepo as IPostRepo);
        const res = await service.createReactionCommentary(reactionDto);

        expect(res.isFailure).to.equal(true);
    })

    it('createReactionCommentary: returns fail result when using invalid data (reaction type)', async () => {
        const reactionDto : ICreatingReactionDTO = { "publicationId": '123', "userId": '1', "reaction": "FAIL" } // FAIL is not a reaction type

        const postRepo = new PostRepo(null);
        //sinon.stub(postRepo, "updateReactionToPost").resolves(null); // Not needed

        const service = new ReactionService(postRepo as IPostRepo);
        const res = await service.createReactionCommentary(reactionDto);

        expect(res.isFailure).to.equal(true);
    })
})