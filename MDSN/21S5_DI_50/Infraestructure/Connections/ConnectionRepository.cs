using System;
using DDDSample1.Domain.Connections;
using DDDSample1.Infrastructure.Shared;
using DDDSample1.Domain.Users;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.Connections
{
    public class ConnectionRepository : BaseRepository<Connection, ConnectionId>, IConnectionRepository
    {
        private readonly DDDSample1DbContext _context;
        private readonly DbSet<Connection> _connections;

        public ConnectionRepository(DDDSample1DbContext context):base(context.Connections)
        {
            _context = context;
            _connections = context.Connections;
        }

        public async Task<List<Connection>> ConnectionsOfUser(UserId userId)
        {
            return await _connections.Where(u => u.OUser.Equals(userId)).ToListAsync();
        }

        public async Task<List<Connection>> bidirecionalConnections(UserId orig, UserId dest)
        {
            return await _connections.Where(c => (c.OUser.Equals(orig) && c.DUser.Equals(dest) )
            ).ToListAsync();
        }
        
        //todo mudar ultima query para repo de users
        public async Task<List<User>> GetPossibleDestinyUsers (UserId userId){
            var connectionsUser = await ConnectionsOfUser(userId);

            List<User> result = new List<User>();

            foreach(Connection connection  in connectionsUser){
                var friendOrigUserId = connection.DUser;
                var friendsConnections = await _connections.Where(c => c.OUser.Equals(friendOrigUserId) && !c.DUser.Equals(userId)).ToListAsync();

                foreach(Connection friend2 in friendsConnections){
                  if(!_verifyIfUserIsFriend(friend2.DUser,userId).Result){
                      result.Add(_context.Users.Where(u => u.Id.Equals(friend2.DUser)).FirstOrDefaultAsync().Result);
                  }
                }
            }

            return result;
        }

        private async Task<bool> _verifyIfUserIsFriend(UserId origUser,UserId userToVerify){
            var connectionsUser = await ConnectionsOfUser(origUser);
            var resultList = connectionsUser.Select(c => c.OUser.Equals(origUser) && c.DUser.Equals(userToVerify)).ToList();
            foreach(bool b in resultList){
                if(b) return true;
            }
            return false;
        }
        
        public async Task<List<UserId>> GetCommonFriends(UserId user1, UserId user2)
        {
            var user1Friends = await Task.Run(()=>_connections
                .Where(connection => connection.OUser.Equals(user1))
                .Select(connection=>connection.DUser).ToHashSet());

            if (!user1Friends.Any())
            {
                return new List<UserId>();
            }
            
            var user2Friends = await Task.Run(()=>_connections
                .Where(connection => connection.OUser.Equals(user2))
                .Select(connection=>connection.DUser).ToHashSet());
            
            return !user2Friends.Any() ? new List<UserId>() : user1Friends.Intersect(user2Friends).ToList();
        }
        
        public async Task<HashSet<UserId>> GetUserFriends(UserId userId)
        {
            var friends = await _connections
                .Where(c => 
                    c.OUser.Equals(userId)
                    || c.DUser.Equals(userId)
                ).ToListAsync();
            
            return friends.Select(connection =>
                    !connection.OUser.Equals(userId)
                        ? connection.OUser
                        : connection.DUser)
                .ToHashSet();
        }
        
        public async Task<int> GetNetworkConnectionStrength(UserId id)
        {
            var connections = await _connections
                .Where(connection => connection.OUser.Equals(id))
                .ToArrayAsync();
            return connections.Select(connection => connection.ConnectionStrength)
                .Sum(c => c.Strength);
        }
        
        public async Task<Dictionary<UserId, int>> GetAllUsersNetworkStrength()
        {
            var dict = new Dictionary<UserId, int>();
            var connections = await _connections.ToListAsync();
            foreach (var c in connections)
            {
                var userdId = c.OUser;
                if (dict.ContainsKey(userdId))
                {
                    continue;
                }
                var val = await GetNetworkConnectionStrength(userdId);
                dict.Add(userdId, val);
            }
            return dict;
        }
        
        public async Task<List<Connection>> BidirecionalConnectionsOfUser(UserId userId)
        {
            return await _connections.Where(u => u.OUser.Equals(userId) || u.DUser.Equals(userId)).ToListAsync();
        }


        public async Task<bool> userAreFriends(UserId origUser, UserId destUser)
        {
            return await this._verifyIfUserIsFriend(origUser, destUser);
        }

        public async Task<Connection> obtainConnection(UserId origUser, UserId destUser){
            return await _connections.Where(connection => connection.OUser.Equals(origUser) && connection.DUser.Equals(destUser)).FirstOrDefaultAsync();
        }
        
    }
}