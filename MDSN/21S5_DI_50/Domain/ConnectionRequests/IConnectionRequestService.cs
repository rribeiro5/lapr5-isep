using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Domain.ConnectionRequests
{
    public interface IConnectionRequestService
    {

        public Task<IEnumerable<ConnectionRequestDTO>> getPendingConnectionsRequestOfUser(UserId id);
        
        public Task<IEnumerable<ConnectionRequestDTO>> GetPendingApprovalRequestsOfUser(UserId id);

        public Task<ConnectionRequestDTO> GetByIdAsync(RequestId id);

        public Task<IEnumerable<ConnectionRequestDTO>> GetAllAsync();

        public Task<ConnectionRequestDTO> UpdateApprovalState(UpdatedApprovalStateRequestDTO dto);
        
        public Task<ConnectionRequestDTO> AddAsync(CreatingRequestDTO dto);

        public Task<UserRequestsDTO> GetRequestsInAcceptance(UserId userId);

        public Task<ResultConnectionDTO> RequestAcceptance(RequestId reqId, RequestAcceptanceDTO answer);

        public Task<ConnectionRequestDTO> CreateIntroductionRequest(CreatingIntroductionRequestDTO dto);
    }
}