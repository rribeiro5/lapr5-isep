using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Connections
{
    public interface IConnectionRepository : IRepository<Connection, ConnectionId>
    {   
        Task<List<Connection>> ConnectionsOfUser(UserId userId);

        Task<List<User>> GetPossibleDestinyUsers (UserId userId);

        Task<List<Connection>> bidirecionalConnections(UserId orig, UserId dest);
        Task<List<UserId>> GetCommonFriends(UserId user1, UserId user2);

        Task<HashSet<UserId>> GetUserFriends(UserId userId);

        Task<Dictionary<UserId, int>> GetAllUsersNetworkStrength();

        Task<List<Connection>> BidirecionalConnectionsOfUser(UserId userId);

        Task<bool> userAreFriends(UserId origUser, UserId destUser);

        Task<Connection> obtainConnection(UserId origUser, UserId destUser);
    }
}