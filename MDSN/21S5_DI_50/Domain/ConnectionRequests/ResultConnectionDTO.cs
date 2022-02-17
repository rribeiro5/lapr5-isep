using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class ResultConnectionDTO
    {
        public Guid User1 { get; set; }
        public Guid User2 { get; set; }
        public ConnectionInfoDTO User1ConnectionInfo { get; set; }
        public ConnectionInfoDTO User2ConnectionInfo { get; set; }

        public ResultConnectionDTO(Guid user1, Guid user2, ConnectionInfoDTO info1, ConnectionInfoDTO info2)
        {
            this.User1 = user1;
            this.User2 = user2;
            this.User1ConnectionInfo = info1;
            this.User2ConnectionInfo = info2;
        }
    }

    public class ConnectionInfoDTO
    {
        public Guid ConnectionId { get; set; } 
        public int ConnectionStrength { get; set; }
        public int RelationshipStrength { get; set; }
        public List<string> Tags { get; set; }

        public ConnectionInfoDTO(Guid connId, int connectionStrength, int relationshipStrength, List<string> tags)
        {
            this.ConnectionId = connId;
            this.ConnectionStrength = connectionStrength;
            this.RelationshipStrength = relationshipStrength;
            this.Tags = new List<string>(tags);
        }
    }
}