using DDDSample1.Domain.Users;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DDDSample1.Domain.Connections
{
    public interface IConnectionService
    {   

        public Task<IEnumerable<ConnectionDTO>> GetAllConnections();

        public Task<IEnumerable<BidirecionalConnectionDTO>> GetAllBidirectionalConnections();
        public Task<UserConnectionsDTO> GetConnectionsOfUser(UserId userId);

        public Task<ConnectionDTO> UpdateConnection(ConnectionId connId, ChangeConnInfoDTO connInfo);

        public Task<IEnumerable<UserDto>> GetPossibleDestinyUsers(UserId userId);

        public Task<ConnectionStrengthsDTO> GetStrengthOfConnection(ConnectionId connId);
    }
}