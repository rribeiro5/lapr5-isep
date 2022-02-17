using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class ConnectionRequest : Entity<RequestId>, IAggregateRoot
    {
        [Required(ErrorMessage ="Required Origin User")]
        public UserId OrigUser { get; private set; }
        public UserId InterUser { get; private set; } // null when is a direct request
        
        [Required(ErrorMessage ="Required Target User")]
        public UserId DestUser { get; private set; }
        public RequestMessage MessageOrigToDest { get; private set; }
        public RequestMessage MessageOrigToInter { get; private set; }
        public RequestMessage MessageInterToDest { get; private set; }
        [Required(ErrorMessage ="Required Strength of Connection")]
        public ConnectionStrength ConnStrengthReq { get; private set; }
        public List<Tag> ConnTagsReq { get; private set; }
        public bool OnApproval { get; private set; }
        public bool InAcceptance { get; private set; }
        public bool Approved { get; private set; }
        public bool Accept { get; private set; }
        

        private ConnectionRequest() {}

        // Introduction constructor
        public ConnectionRequest(UserId origUser, UserId interUser, UserId destUser, string messageOrigToDest, string messageOrigToInter, int connStrength, List<string> connTags) 
        {
            this.Id = new RequestId(Guid.NewGuid());
            if (origUser == null || interUser == null || destUser == null)
                throw new BusinessRuleValidationException("None of the intervening users can be null");
            this.OrigUser = origUser;
            this.InterUser = interUser;
            this.DestUser = destUser;
            this.MessageOrigToDest = new RequestMessage(messageOrigToDest);
            this.MessageOrigToInter = new RequestMessage(messageOrigToInter);
            this.MessageInterToDest = null;
            this.ConnStrengthReq = new ConnectionStrength(connStrength);
            this.ConnTagsReq = connTags.ConvertAll<Tag>(e => new Tag(e));
            this.OnApproval = true;
            this.InAcceptance = false;
        }

        // Direct request constructor
        public ConnectionRequest(UserId origUser, UserId destUser, string messageOrigToDest, int connStrength, List<string> connTags)
        {
            this.Id = new RequestId(Guid.NewGuid());
            if (origUser == null || destUser == null)
                throw new BusinessRuleValidationException("None of the intervening users can be null");
            this.OrigUser = origUser;
            this.InterUser = null;
            this.DestUser = destUser;
            this.MessageOrigToDest = new RequestMessage(messageOrigToDest);
            this.MessageOrigToInter = null;
            this.MessageInterToDest = null;
            this.ConnStrengthReq = new ConnectionStrength(connStrength);
            this.ConnTagsReq = connTags.ConvertAll<Tag>(e => new Tag(e));
            this.OnApproval = false;
            this.InAcceptance = true;
        }

        public void Approve(string messageInterToDest)
        {
            this.MessageInterToDest = new RequestMessage(messageInterToDest);
            this.OnApproval = false;
            this.Approved = true;
            this.InAcceptance = true;
        }

        public void Disapproved()
        {
            this.OnApproval = false;
            this.Approved = false;
            this.InAcceptance = false;
        }

        public void AcceptRequest()
        {
            this.InAcceptance = false;
            this.Accept = true;
        }

        public void RejectRequest()
        {
            this.InAcceptance = false;
            this.Accept = false;
        }
    }
}