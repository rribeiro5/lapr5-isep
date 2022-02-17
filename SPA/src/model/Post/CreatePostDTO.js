export default class CreatePostDTO{
    constructor(userId,text,tags) {
        this.userId=userId;
        this.text=text;
        this.tags=tags;
    }
}