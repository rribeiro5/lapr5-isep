using System;
using System.Collections.Generic;
using DDDSample1.Domain.Users;


//todo:Review guid vs userid
namespace DDDSample1.Domain.ConnectionRequests
{
    public class CreatingRequestDTO
    {
        public Guid OrigUser { get; set; }
        public Guid DestUser { get; set; }
        public string MessageOrigToDest { get; set; }
        public int ConnStrengthReq { get; set; }
        public List<string> ConnTagsReq { get; set; }
        

        public CreatingRequestDTO(Guid origUser, Guid destUser, string messageOrigToDest, int connStrengthReq, List<string> connTagsReq)
        {
            OrigUser = origUser;
            DestUser = destUser;
            MessageOrigToDest = messageOrigToDest;
            ConnStrengthReq = connStrengthReq;
            ConnTagsReq = connTagsReq;

        }
    }
}