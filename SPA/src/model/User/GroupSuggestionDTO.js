export default class GroupSuggestionDTO{
    constructor(userId,lTagsObrigatorias,nTagsComum,nMinimoUsers,desired,toAvoid) {
        this.userId = userId
        this.lTagsObrigatorias = lTagsObrigatorias
        this.nTagsComum = nTagsComum
        this.nMinimoUsers = nTagsComum
        this.desired = desired
        this.toAvoid = toAvoid
    }
}