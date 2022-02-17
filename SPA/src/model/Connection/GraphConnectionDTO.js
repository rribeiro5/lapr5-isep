export default class GraphConnectionDTO {
    constructor(origUser,destUser,origPos,destPos,connectionStrength,relationshipStrength,connTags) {
        this.origUser = origUser
        this.destUser = destUser
        this.pointX = origPos
        this.pointY = destPos
        this.connectionStrength = connectionStrength
        this.relationshipStrength = relationshipStrength
        this.connTags = connTags
        this.inPath = false
    }
}