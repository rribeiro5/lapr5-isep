using System;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Connections
{

    public class ConnectionStrengthsDTO
    {
        public Guid Id { get; set; }
        
        public int ConnectionStrength { get; set; }
        
        public int RelationshipStrength { get; set; }

        public ConnectionStrengthsDTO(Guid id, int connectionStrength, int relationshipStrength)
        {
            this.Id = id;
            this.ConnectionStrength = connectionStrength;
            this.RelationshipStrength = relationshipStrength;
        }
    }
}