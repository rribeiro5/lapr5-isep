import {CommentText} from "../../../../src/domain/Comment/CommentText";
import {expect} from "chai";
import {PostText} from "../../../../src/domain/postText";

describe('PostText Unit Tests',()=>{
  it('create valid post text',()=>{
    const value="text"
    const text=PostText.create(value);
    expect(text.isSuccess).to.equal(true);
    expect(text.getValue().value).to.equal(value);
  })

  it('fail to create post text with empty string', () => {
    const value = ""
    const text = PostText.create(value);
    expect(text.isFailure).to.equal(true);
  })

  it('fail to create post text with null string', () => {
    const value = null
    const text = PostText.create(value);
    expect(text.isFailure).to.equal(true);
  })

  it('fail to create post text with undefined string', () => {
    const value = undefined
    const text = PostText.create(value);
    expect(text.isFailure).to.equal(true);
  })

  it('fail to create post text with undefined string', () => {
    const value = undefined
    const text = PostText.create(value);
    expect(text.isFailure).to.equal(true);
  })

  it('fail to create post text with string length > 10000', () => {
    const value = makestr(10001)
    const text = PostText.create(value);
    expect(text.isFailure).to.equal(true);
  })

  function makestr(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ';
    var charactersLength = characters.length;
    for ( var i = 0; i < length; i++ ) {
      result += characters.charAt(Math.floor(Math.random() *
        charactersLength));
    }
    return result;
  }

})
