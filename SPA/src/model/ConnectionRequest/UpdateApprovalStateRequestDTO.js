export default class UpdateApprovalStateRequestDTO {
    constructor(id,messageInterToDest, approved) {
        this.Id = id
        this.Approved = approved
        this.MessageInterToDest  = messageInterToDest
    }
}