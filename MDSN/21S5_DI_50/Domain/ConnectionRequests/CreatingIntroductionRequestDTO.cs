using System;
using System.Collections.Generic;
using DDDSample1.Domain.Users;


//todo:Review guid vs userid
namespace DDDSample1.Domain.ConnectionRequests
{
    public class CreatingIntroductionRequestDTO
    {
        public Guid OrigUser { get; set; }

        public Guid InterUser { get; set; }

        public Guid DestUser { get; set; }
        
        public string MessageOrigToDest { get; set; }

        public string MessageOrigToInter { get; set; }

        public int ConnectionStrength {get;set;}

        public List<string> Tags { get; set; }
        

        public CreatingIntroductionRequestDTO(Guid origUser,Guid interUser,Guid destUser, string messageOrigToDest, string messageOrigToInter,int connectionStrength ,List<string> connTagsReq)
        {
            this.OrigUser = origUser;
            this.InterUser = interUser;
            this.DestUser = destUser;
            this.MessageOrigToDest = messageOrigToDest;
            this.MessageOrigToInter = messageOrigToInter;
            this.ConnectionStrength = connectionStrength;
            this.Tags = connTagsReq;

        }
    }
}