using System;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Utils;
using Tag = DDDSample1.Domain.Users.Tag;

namespace DDDSample1.Domain.Connections
{
    public class Connection : Entity<ConnectionId>, IAggregateRoot
    {
        
        public UserId OUser { get; private set; }
        public UserId DUser { get; private set; }

        public ICollection<Tag> Tags{ get; private set; }
        
        
        public ConnectionStrength ConnectionStrength { get; private set; }

        
        public RelationshipStrength RelationshipStrength { get; private set; }

        private Connection(){}

        public Connection(int connectionStrength, int relationshipStrength, List<string> tags, UserId oUser, UserId dUser)
        {
            this.Id = new ConnectionId(Guid.NewGuid());
            this.ConnectionStrength = new ConnectionStrength(connectionStrength);
            this.RelationshipStrength = new RelationshipStrength(relationshipStrength);
            this.OUser = oUser;
            this.DUser = dUser;
            GenerateTags(tags);
        }

        public void UpdateConnStrengthTags(int connStrength, List<string> tags)
        {
            this.ConnectionStrength = new ConnectionStrength(connStrength);
            GenerateTags(tags);
        }

        private void GenerateTags(List<string> tags)
        {
            this.Tags = new List<Tag>();
            if (!tags.IsNullOrEmpty())
            {
                foreach (var xTag in tags)
                {
                    this.Tags.Add(new Tag(xTag));
                }
            }
        }

        public void incrementRelationshipStrength()
        {   
            int newValue = this.RelationshipStrength.Strength + 1;
            this.RelationshipStrength = new RelationshipStrength(newValue);
        }

        public void decrementRelationshipStrength()
        {   
            int newValue = this.RelationshipStrength.Strength - 1;
            this.RelationshipStrength = new RelationshipStrength(newValue);
        }
    }
    
}