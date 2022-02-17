import IReactionDTO from "../../../../src/dto/IReactionDTO";
import {Post} from "../../../../src/domain/post";
import {expect} from "chai";
import IPostDTO from "../../../../src/dto/IPostDTO";

describe('Post Unit Tests', () => {
  let userId: string = "1234"
  let text: string = "text"
  let reactions: IReactionDTO[] = []
  let postDTO: IPostDTO = {
    id: "",
    userId: userId,
    text: text,
    reactions: reactions,
    tags: [],
    comments: [],
    creationDateTime: 0
  }

  const resetUserId = () => postDTO.userId = userId;
  const resetText = () => postDTO.text = text;
  const resetReactions = () => postDTO.reactions = reactions;

  it('create valid post', () => {
    const post = Post.create(postDTO);
    expect(post.isSuccess).to.equal(true);
    expect(post.getValue().userId.id).to.equal(userId);
    expect(post.getValue().text.value).to.equal(text);
    for (let i = 0; i < reactions.length; i++) {
      expect(post.getValue().reactions[i]).to.equal(reactions[i]);
    }
  })

  it('fail to create post with empty userId', () => {
    postDTO.userId = ""
    const dto = Post.create(postDTO);
    expect(dto.isFailure).to.equal(true);
    resetUserId()
  })

  it('fail to create post with null userId', () => {
    postDTO.userId = null
    const dto = Post.create(postDTO);
    expect(dto.isFailure).to.equal(true);
    resetUserId()
  })

  it('fail to create post with undefined userId', () => {
    postDTO.userId = undefined
    const dto = Post.create(postDTO);
    expect(dto.isFailure).to.equal(true);
    resetUserId()
  })

  it('fail to create post with empty text', () => {
    postDTO.text = ""
    const post = Post.create(postDTO);
    expect(post.isFailure).to.equal(true);
    resetText()
  })

  it('fail to create post with invalid text', () => {
    postDTO.text = null
    const post = Post.create(postDTO);
    expect(post.isFailure).to.equal(true);
    resetText()
  })

  it('Create post with undefined reactions', () => {
    postDTO.reactions = undefined
    const post = Post.create(postDTO);
    expect(post.isSuccess).to.equal(true);
    resetReactions()
  })

  it('Create post with undefined comments', () => {
    postDTO.comments = undefined
    const post = Post.create(postDTO);
    expect(post.isSuccess).to.equal(true);
    resetReactions()
  })

})
