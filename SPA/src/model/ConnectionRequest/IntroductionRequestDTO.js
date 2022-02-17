
export default class IntroductionRequestDTO{
    constructor(OrigUser,InterUser,DestUser,MessageOrigToDest,MessageOrigToInter,ConnectionStrength,Tags) {
        this.OrigUser = OrigUser
        this.InterUser = InterUser
        this.DestUser = DestUser
        this.MessageOrigToDest = MessageOrigToDest
        this.MessageOrigToInter = MessageOrigToInter
        this.ConnectionStrength = ConnectionStrength
        this.Tags = Tags
    }
}