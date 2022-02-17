using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.Users
{
    public class UserRepository : BaseRepository<User, UserId>,IUserRepository
    {
        private readonly DDDSample1DbContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(DDDSample1DbContext context):base(context.Users)
        {
            _context = context;
            _users = context.Users;
        }
        //todo devolver logo pela bd os n users sugeridos pelo sistema
        public async Task<IList<User>> GetAllNonFriendsAndWithoutFriendRequests(UserId userId)
        {
            var pendingUserRequests =  await _getUsersFromPendingRequests(userId);
            var friends = await GetUserFriends(userId);
            return await _users
                .Where(u => !u.Id.Equals(userId) 
                            && u.Active==true
                            && !pendingUserRequests.Contains(u.Id)
                            && !friends.Contains(u.Id)
                ).ToListAsync();
        }
        
        public async Task<IList<User>> GetAllNonFriendsAndWithoutFriendRequests(UserId userId, 
            IEnumerable<UserId>pendingUserRequests,IEnumerable<UserId>friends)
        {
            return await _users
                .Where(u => !u.Id.Equals(userId) 
                            && u.Active==true
                            && !pendingUserRequests.Contains(u.Id)
                            && !friends.Contains(u.Id)
                ).ToListAsync();
        }
        //todo mudar para repo de connections
        public async Task<HashSet<UserId>> GetUserFriends(UserId userId)
        {
            var friends = await _context.Connections
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
        //todo mudar para repo de connectionRequests
        private async Task<HashSet<UserId>> _getUsersFromPendingRequests(UserId userId)
        {
            var connectionRequests = _context.ConnectionRequests;
            var pendingUserRequests = await connectionRequests
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

         public async Task<IList<User>> GetMutualFriends(UserId origUserId , UserId destUserId){

            var origFriends = await this.GetUserFriends(origUserId);
            var destFriends = await this.GetUserFriends(destUserId);

            List<UserId> mutualFriendsId = new List<UserId>();

            foreach(UserId origFriend in origFriends){
                if(origFriend.Equals(destUserId) || !destFriends.Contains(origFriend)) continue;
                mutualFriendsId.Add(origFriend);
            }

            return await this.GetByIdsAsync(mutualFriendsId);
        }

        public async Task<bool> validIntroductionRequest(UserId origUserId, UserId interUserId , UserId destUserId)
        {   
            // verify if any user is duplicated , if it is then it's valid
            if(destUserId.Equals(origUserId) ||destUserId.Equals(interUserId) || origUserId.Equals(interUserId))
                throw new BusinessRuleValidationException("Can't send request to the same person");

            var mutualFriends = await this.GetMutualFriends(origUserId,destUserId);
            var interUser = await this.GetByIdAsync(interUserId);

            // verify if the intermediate user for Introduction Request has Connection with both users
            if(interUser == null || !mutualFriends.Contains(interUser)) 
                throw new BusinessRuleValidationException("Intermediate User doesn't have a connection with both users");

            var origFriends = await this.GetUserFriends(origUserId);    

            // verify if the user that makes request is not already friend with end user
            if(origFriends.Contains(destUserId))
                throw new BusinessRuleValidationException("Origin User already has connection with end user");

            return true;
        }

        
        public async Task<IList<User>> GetUserByName(string name)
        {
            return await _users
                .Where(u => u._Name._Name.Equals(name)
                ).ToListAsync();
        }
        

        
        //todo: melhorar
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _users
                .Where(u => u._Email._Email.Equals(email)
                ).FirstOrDefaultAsync();
            return user;
        }
       
        public async Task<IList<User>> GetUsersByTags(IList<string> tags)
        {
            IList<Tag> lst = tags.Select(tag => new Tag(tag)).ToList();
            return await _users
                .Where(u => u._InterestTags.Intersect(lst).Equals(lst)
                ).ToListAsync();
        }

        public async Task<IList<User>> GetUsersByCountry(string country)
        {
            return await _users.Where(u => u._Country._Country.Equals(country))
                .ToListAsync();
        }
        //todo mudar para repo de connections
        public async Task<int> GetNetworkConnectionStrength(UserId id)
        {
            var connections = await _context.Connections
                .Where(connection => connection.OUser.Equals(id))
                .ToArrayAsync();
            return connections.Select(connection => connection.ConnectionStrength)
                .Sum(c => c.Strength);
        }
        
        //todo melhorar para usar o count
        public async Task<Dictionary<Tag, int>> GetUserTagCloud(UserId id)
        {
            var user = await _users.Where(u => u.Id.Equals(id))
                .FirstOrDefaultAsync();
            var tags = user._InterestTags;
            var tagCountDict = new Dictionary<Tag, int>();
            TagListToMap(tags, tagCountDict);
            return tagCountDict;
        }

        public async Task<Dictionary<Tag, int>> GetUsersTagCloud()
        {
            var tagsLists = await _users.Select(u => u._InterestTags.ToArray())
                .AsNoTracking()
                .ToArrayAsync();
            
            var tagCountDict = new Dictionary<Tag, int>();

            foreach (var list in tagsLists)
            {
                TagListToMap(list, tagCountDict);
            }
            return tagCountDict;
        }

        private static void TagListToMap(IEnumerable<Tag> list, IDictionary<Tag, int> tagCountDict)
        {
            foreach (var tag in list)
            {
                if (!tagCountDict.ContainsKey(tag))
                { 
                    tagCountDict[tag] = 1;
                }
                else
                {
                    tagCountDict[tag] += 1;
                }
            }
        }
    }
}