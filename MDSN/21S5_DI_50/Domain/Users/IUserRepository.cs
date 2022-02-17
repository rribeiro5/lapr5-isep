using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;


namespace DDDSample1.Domain.Users
{
    public interface IUserRepository: IRepository<User,UserId>
    {
        public  Task<IList<User>> GetAllNonFriendsAndWithoutFriendRequests(UserId userId);
        
        public Task<IList<User>> GetUserByName(string name);
        
        public Task<User> GetUserByEmail(string email);
        
        public Task<IList<User>> GetUsersByTags(IList<string> tags);

        public Task<IList<User>> GetUsersByCountry(string country);

        public Task<IList<User>> GetMutualFriends(UserId origUserId , UserId destUserId);

        public  Task<bool> validIntroductionRequest(UserId origUserId, UserId interUserId , UserId destUserId);

        Task<int> GetNetworkConnectionStrength(UserId id);

        Task<HashSet<UserId>> GetUserFriends(UserId userId);

        Task<IList<User>> GetAllNonFriendsAndWithoutFriendRequests(UserId userId,
            IEnumerable<UserId> pendingUserRequests, IEnumerable<UserId> friends);

        Task<Dictionary<Tag, int>> GetUserTagCloud(UserId id);
        
        Task<Dictionary<Tag,int>> GetUsersTagCloud();
    }
}