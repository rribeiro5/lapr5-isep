
export default class CreacteDirectRequestDTO {
    constructor(origUser,destUser,messageOrigToDest, connStrengthReq,connTagsReq) {
        this.origUser = origUser;
        this.destUser = destUser;
        this.messageOrigToDest = messageOrigToDest;
        this.connStrengthReq = connStrengthReq;
        this.connTagsReq = connTagsReq;
    }
}


