export default class UserNodeDTO {
    constructor(id,name, avatar,userLevel,email,interestTags,emotionalState,position,color) {
        this.id = id
        this.name = name
        this.avatar = avatar
        this.userLevel = userLevel
        this.email = email
        this.interestTags = interestTags
        this.emotionalState = emotionalState
        this.position = position
        this.color = color
    }
}