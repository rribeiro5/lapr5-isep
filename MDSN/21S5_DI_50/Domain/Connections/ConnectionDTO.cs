using System;
using System.Collections.Generic;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Connections
{
    public class ConnectionDTO
    {
        public Guid Id { get; set; }
        
        public UserId OUser { get; set; }
        public UserDto OrigUser { get; set; }
        public UserId DUser { get; set; }
        public UserDto DestUser { get; set; }

        public ICollection<string> Tags{ get; set; }
        
        
        public int ConnectionStrength { get; set; }

        
        public int RelationshipStrength { get; set; }

        public ConnectionDTO(Guid id, UserId oUser, UserId dUser, ICollection<string> tags, int connectionStrength, int relationshipStrength)
        {
            this.Id = id;
            this.OUser = oUser;
            this.OrigUser = null;
            this.DUser = dUser;
            this.DestUser = null;
            this.Tags = tags;
            this.ConnectionStrength = connectionStrength;
            this.RelationshipStrength = relationshipStrength;
        }
        public ConnectionDTO(Guid id, UserId oUser, UserDto origUser, UserId dUser, UserDto destUser, ICollection<string> tags, int connectionStrength, int relationshipStrength)
        {
            this.Id = id;
            this.OUser = oUser;
            this.OrigUser = origUser;
            this.DUser = dUser;
            this.DestUser = destUser;
            this.Tags = tags;
            this.ConnectionStrength = connectionStrength;
            this.RelationshipStrength = relationshipStrength;
        }
    }
}