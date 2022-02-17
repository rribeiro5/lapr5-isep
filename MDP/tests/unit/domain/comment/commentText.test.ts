import { expect } from 'chai';
import {CommentText} from '../../../../src/domain/Comment/CommentText';

describe('Comment Unit Tests',()=>{
    it('create valid comment text',()=>{
        const value="text"
        const text=CommentText.create(value);
        expect(text.isSuccess).to.equal(true);
        expect(text.getValue().value).to.equal(value);
    })

    it('fail to create comment text with empty string', () => {
        const value = ""
        const text = CommentText.create(value);
        expect(text.isFailure).to.equal(true);
    })

    it('fail to create comment text with null string', () => {
        const value = null
        const text = CommentText.create(value);
        expect(text.isFailure).to.equal(true);
    })

    it('fail to create comment text with undefined string', () => {
        const value = undefined
        const text = CommentText.create(value);
        expect(text.isFailure).to.equal(true);
    })
})