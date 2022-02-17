export default class Post{
    constructor(id,userId,text,tags,creationDateTime) {
        this.id=id;
        this.userId=userId;
        this.text=text;
        this.tags=tags;
        this.creationDateTime=creationDateTime;
    }
}