export default class ConnectionRequest {
    constructor(id,origUser,oUserInfo,interUser,iUserInfo,destUser,dUserInfo,messageOrigToDest,messageOrigToInter,messageInterToDest) {
        this.id = id
        this.origUser = origUser
        this.oUserInfo = oUserInfo
        this.interUser = interUser
        this.iUserInfo = iUserInfo
        this.destUser = destUser
        this.dUserInfo = dUserInfo
        this.messageOrigToDest = messageOrigToDest
        this.messageOrigToInter = messageOrigToInter
        this.messageInterToDest = messageInterToDest
    }
}