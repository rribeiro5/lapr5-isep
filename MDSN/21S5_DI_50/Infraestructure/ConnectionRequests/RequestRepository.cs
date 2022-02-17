using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure.Shared;
using DDDSample1.Domain.Users;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.ConnectionRequests
{
    public class RequestRepository : BaseRepository<ConnectionRequest, RequestId>, IRequestRepository
    {
        //private readonly DDDSample1DbContext _context;
        private readonly DbSet<ConnectionRequest> _connectionsRequests;
        public RequestRepository(DDDSample1DbContext context):base(context.ConnectionRequests)
        {
            //this._context = context;
            _connectionsRequests = context.ConnectionRequests;
        }

        public async Task<IList<ConnectionRequest>> getAllRequestsOfUser(UserId userId){

            return await _connectionsRequests.Where(request => 
            ((request.InterUser.Equals(userId))||
            (request.DestUser.Equals(userId)  )|| request.OrigUser.Equals(userId)))
            .ToListAsync();
        }


        /// <sumary>
        ///  Method that returns all the pending connections from the user
        /// Metodo que retorna todos pedidos de ligacao (diretas ou inderetas)
        /// </sumary>
        public async Task<IList<ConnectionRequest>> getPendingConnectionsRequestOfUser(UserId userId){

            return await _connectionsRequests.Where(request => 
            (request.OnApproval && request.InterUser.Equals(userId))||
            (request.DestUser.Equals(userId) && request.InAcceptance))
            .ToListAsync();
        }
        
        public async Task<IList<ConnectionRequest>> GetPendingApprovalRequestsOfUser(UserId userId){

            return await _connectionsRequests.Where(request => 
                    (request.OnApproval && request.InterUser.Equals(userId)))
                .ToListAsync();
        }

        public async Task<List<ConnectionRequest>> GetRequestsInAcceptanceOfUser(UserId userId)
        {
            return await _connectionsRequests.Where(req => 
                req.InAcceptance && req.DestUser.Equals(userId)).ToListAsync(); // todo: verificar que é introduçao
        }

         public async Task<ConnectionRequest> RegisterConnectionRequest(ConnectionRequest connectionRequest){
             var pendingConnections = await this.getPendingConnectionsRequestRegardlessStatus(connectionRequest.DestUser);
             foreach(ConnectionRequest pending in pendingConnections){
                 if(pending.OrigUser.Equals(connectionRequest.OrigUser))throw new BusinessRuleValidationException("Request already made by origin user");
             }
             return await this.AddAsync(connectionRequest);
         }

          private async Task<IList<ConnectionRequest>> getPendingConnectionsRequestRegardlessStatus(UserId userId)
            => await _connectionsRequests.Where(req => req.DestUser.Equals(userId)).ToListAsync();

        
          public async Task<HashSet<UserId>> GetUsersFromPendingRequests(UserId userId)
          {
              //var connectionRequests = _context.ConnectionRequests;
              var pendingUserRequests = await _connectionsRequests
                  .Where(c =>
                      c.InAcceptance
                      && ( c.OrigUser.Equals(userId)
                           ||c.DestUser.Equals(userId)
                      )
                  ).ToListAsync();
            
              return pendingUserRequests.Select(pendingUserRequest => 
                      !pendingUserRequest.OrigUser.Equals(userId)
                          ? pendingUserRequest.OrigUser
                          : pendingUserRequest.DestUser)
                  .ToHashSet();
          }
    }
}