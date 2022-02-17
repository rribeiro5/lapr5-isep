using System;
using System.Collections.Generic;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class ConnectionRequestDTO
    {
        public Guid Id { get; set; }
        public UserId OrigUser { get; set; }
        public UserDto OUser { get; set; }
        public UserId InterUser { get; set; } // null when is a direct request
        public UserDto IUser { get; set; }
        public UserId DestUser { get; set; }
        public UserDto DUser { get; set; }
        public string MessageOrigToDest { get; set; }
        public string MessageOrigToInter { get; set; }
        public string MessageInterToDest { get; set; }

        public ConnectionRequestDTO(Guid Id, UserId origUser, UserId interUser, UserId destUser, string messageOrigToDest, string messageOrigToInter, string messageInterToDest)
        {
            this.Id = Id;
            this.OrigUser = origUser;
            this.OUser = null;
            this.InterUser = interUser;
            this.IUser = null;
            this.DestUser = destUser;
            this.DUser = null;
            this.MessageOrigToDest = messageOrigToDest;
            this.MessageOrigToInter = messageOrigToInter;
            this.MessageInterToDest = messageInterToDest;
        }
        public ConnectionRequestDTO(Guid Id, UserId origUser, UserDto oUser, UserId interUser, UserDto iUser, UserId destUser, UserDto dUser, string messageOrigToDest, string messageOrigToInter, string messageInterToDest)
        {
            this.Id = Id;
            this.OrigUser = origUser;
            this.OUser = oUser;
            this.InterUser = interUser;
            this.IUser = iUser;
            this.DestUser = destUser;
            this.DUser = dUser;
            this.MessageOrigToDest = messageOrigToDest;
            this.MessageOrigToInter = messageOrigToInter;
            this.MessageInterToDest = messageInterToDest;
        }
        // introcutionRequest
         public ConnectionRequestDTO(Guid Id, UserId origUser, UserId interUser, UserId destUser, string messageOrigToDest, string messageOrigToInter)
        {
            this.Id = Id;
            this.OrigUser = origUser;
            this.OUser = null;
            this.InterUser = interUser;
            this.IUser = null;
            this.DestUser = destUser;
            this.DUser = null;
            this.MessageOrigToDest = messageOrigToDest;
            this.MessageOrigToInter = messageOrigToInter;
        }
        public ConnectionRequestDTO(Guid Id, UserId origUser, UserDto oUser, UserId interUser, UserDto iUser, UserId destUser, UserDto dUser, string messageOrigToDest, string messageOrigToInter)
        {
            this.Id = Id;
            this.OrigUser = origUser;
            this.OUser = oUser;
            this.InterUser = interUser;
            this.IUser = iUser;
            this.DestUser = destUser;
            this.DUser = dUser;
            this.MessageOrigToDest = messageOrigToDest;
            this.MessageOrigToInter = messageOrigToInter;
        }

        public ConnectionRequestDTO()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectionRequestDTO dTO &&
                   Id.Equals(dTO.Id) &&
                   EqualityComparer<UserId>.Default.Equals(OrigUser, dTO.OrigUser) &&
                   EqualityComparer<UserId>.Default.Equals(InterUser, dTO.InterUser) &&
                   EqualityComparer<UserId>.Default.Equals(DestUser, dTO.DestUser) &&
                   MessageOrigToDest == dTO.MessageOrigToDest &&
                   MessageOrigToInter == dTO.MessageOrigToInter &&
                   MessageInterToDest == dTO.MessageInterToDest;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, OrigUser, InterUser, DestUser, MessageOrigToDest, MessageOrigToInter, MessageInterToDest);
        }
    }
}