import { expect } from 'chai';
import { Comment } from '../../../../src/domain/Comment/Comment';
import ICreatingCommentDTO from '../../../../src/dto/ICreatingCommentDTO';
import IReactionDTO from '../../../../src/dto/IReactionDTO';

describe('Comment Unit Tests', () => {
    let userId : string="1234"
    let text : string ="text"
    let reactions: IReactionDTO[]=[]
    let commentDto:ICreatingCommentDTO={
        postId:"1",
        userId:userId,
        text:text,
        reactions:reactions
    }

    const resetUserId=()=>commentDto.userId=userId;
    const resetText = () => commentDto.text = text;
    const resetReactions = () => commentDto.reactions = reactions;

    it('create valid comment', () => {
        const comment = Comment.create(commentDto);
        expect(comment.isSuccess).to.equal(true);
        expect(comment.getValue().userId.id).to.equal(userId);
        expect(comment.getValue().text.value).to.equal(text);
        for (let i = 0; i < reactions.length;i++){
            expect(comment.getValue().reactions[i]).to.equal(reactions[i]);
        }  
    })

    it('fail to create comment with empty userId', () => {
        commentDto.userId=""
        const dto = Comment.create(commentDto);
        expect(dto.isFailure).to.equal(true);
        resetUserId()
    })

    it('fail to create comment with null userId', () => {
        commentDto.userId = null
        const dto = Comment.create(commentDto);
        expect(dto.isFailure).to.equal(true);
        resetUserId()
    })

    it('fail to create comment with undefined userId', () => {
        commentDto.userId = undefined
        const dto = Comment.create(commentDto);
        expect(dto.isFailure).to.equal(true);
        resetUserId()
    })

    it('fail to create comment with empty text', () => {
        commentDto.text=""
        const comment = Comment.create(commentDto);
        expect(comment.isFailure).to.equal(true);
        resetText()
    })

    it('fail to create comment with invalid text', () => {
        commentDto.text = null
        const comment = Comment.create(commentDto);
        expect(comment.isFailure).to.equal(true);
        resetText()
    })

    it('Create comment with null reactions', () => {
        commentDto.reactions = null
        const comment = Comment.create(commentDto);
        expect(comment.isSuccess).to.equal(true);
        resetReactions()
    })

    it('Create comment with undefined reactions', () => {
        commentDto.reactions = undefined
        const comment = Comment.create(commentDto);
        expect(comment.isSuccess).to.equal(true);
        resetReactions()
    })
})