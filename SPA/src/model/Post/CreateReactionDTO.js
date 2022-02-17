export default class CreateReactionDTO{
    constructor(publicationId,userId,reaction) {
        this.publicationId = publicationId;
        this.userId = userId;
        this.reaction = reaction;
    }
}