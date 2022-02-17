import { expect } from 'chai';

import Reaction from '../../../../src/domain/Reaction/Reaction';
import IReactionDTO from '../../../../src/dto/IReactionDTO';

describe('Reaction Unit Tests', () => {
    it('create valid reaction', () => {
        const dto : IReactionDTO = { userId: '123', reaction: 'LIKE' };
        const reaction = Reaction.create(dto);

        expect(reaction.isSuccess).to.equal(true);
        expect(reaction.getValue().userId.id).to.equal(dto.userId);
        expect(reaction.getValue().reactionType.value).to.equal(dto.reaction);
    })

    it('fail creating reaction with invalid reaction type', () => {
        const dto : IReactionDTO = { userId: '123', reaction: 'FAIL' };
        const reaction = Reaction.create(dto);

        expect(reaction.isFailure).to.equal(true);
    })

    it('fail creating reaction with null reaction type', () => {
        const dto : IReactionDTO = { userId: '123', reaction: null };
        const reaction = Reaction.create(dto);

        expect(reaction.isFailure).to.equal(true);
    })

    it('fail creating reaction with null user id', () => {
        const dto : IReactionDTO = { userId: null, reaction: 'LIKE' };
        const reaction = Reaction.create(dto);

        expect(reaction.isFailure).to.equal(true);
    })
})