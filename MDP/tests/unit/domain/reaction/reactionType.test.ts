import { expect } from 'chai';

import ReactionType from '../../../../src/domain/Reaction/ReactionType';

describe('Reaction Type Unit Tests', () => {
    it('create valid reaction type (LIKE)', () => {
        const reaction = "LIKE";
        const reactionType = ReactionType.create(reaction);

        expect(reactionType.isSuccess).to.equal(true);
        expect(reactionType.getValue().value).to.equal(reaction);
    })

    it('create valid reaction type (DISLIKE)', () => {
        const reaction = "DISLIKE";
        const reactionType = ReactionType.create(reaction);

        expect(reactionType.isSuccess).to.equal(true);
        expect(reactionType.getValue().value).to.equal(reaction);
    })

    it('fail creating null reaction type', () => {
        const reaction = null;
        const reactionType = ReactionType.create(reaction);

        expect(reactionType.isFailure).to.equal(true);
    })

    it('fail creating invalid reaction type', () => {
        const reaction = "FAIL"; // Not valid
        const reactionType = ReactionType.create(reaction);

        expect(reactionType.isFailure).to.equal(true);
    })
})