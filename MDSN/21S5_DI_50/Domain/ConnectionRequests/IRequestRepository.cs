using DDDSample1.Domain.Shared;
using System.Collections.Generic;
using DDDSample1.Domain.Users;
using System.Threading.Tasks;

namespace DDDSample1.Domain.ConnectionRequests
{
    public interface IRequestRepository : IRepository<ConnectionRequest, RequestId>
    {   
        public  Task<IList<ConnectionRequest>> getAllRequestsOfUser(UserId userId);
        public  Task<IList<ConnectionRequest>> getPendingConnectionsRequestOfUser(UserId userId);

        public  Task<IList<ConnectionRequest>> GetPendingApprovalRequestsOfUser(UserId userId);
        
        public Task<List<ConnectionRequest>> GetRequestsInAcceptanceOfUser(UserId userId);

        public Task<ConnectionRequest> RegisterConnectionRequest(ConnectionRequest connectionRequest);

        Task<HashSet<UserId>> GetUsersFromPendingRequests(UserId userId);
    }
}