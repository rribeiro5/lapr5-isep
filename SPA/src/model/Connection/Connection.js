export default class Connection {
    constructor(id,origId,origUser,destId,destUser,connectionStrength,relStrength,tags) {
        this.id = id
        this.origId = origId
        this.origUser = origUser
        this.destId = destId
        this.destUser = destUser
        this.connectionStrength = connectionStrength
        this.relStrength = relStrength
        this.tags = tags
    }
}