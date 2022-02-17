using System;
using System.Collections.Generic;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Connections
{
    public class BidirecionalConnectionDTO
    {

        
        public Guid OUser { get; set; }
        public Guid DUser { get; set; }

        
        public int ConnectionStrengthOUser { get; set; }

        
        public int ConnectionStrengthDUser { get; set; }

        public int RelationshipStrengthOUser {get;set;}

        public int RelationshipStrengthDUser {get;set;}

        public BidirecionalConnectionDTO(Guid oUser, Guid dUser, int connectionStrengthOUser, int connectionStrengthDUser,int relationshipStrengthOUser,int relationshipStrengthDUser)
        {
           this.OUser = oUser;
           this.DUser = dUser;
           this.ConnectionStrengthOUser = connectionStrengthOUser;
           this.ConnectionStrengthDUser = connectionStrengthDUser;
           this.RelationshipStrengthOUser = relationshipStrengthOUser;
           this.RelationshipStrengthDUser = relationshipStrengthDUser;
        }
    }
}