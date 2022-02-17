using System;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class UpdatedApprovalStateRequestDTO
    {
        public Guid Id { get; set; }
        
        public bool Approved { get; set; }
        
        public string MessageInterToDest { get; set; }

        public UpdatedApprovalStateRequestDTO(Guid id, bool approved, string messageInterToDest)
        {
            Id = id;
            Approved = approved;
            MessageInterToDest = messageInterToDest;
        }
    }
}