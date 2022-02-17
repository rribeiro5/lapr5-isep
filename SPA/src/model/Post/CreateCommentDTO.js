export default class CreateCommenttDTO {
    constructor(postId, userId, text) {
        this.postId = postId;
        this.userId = userId;
        this.text = text;
    }
}