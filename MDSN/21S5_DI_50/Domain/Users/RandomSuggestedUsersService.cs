using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Connections;

namespace DDDSample1.Domain.Users
{
    public class RandomSuggestedUsersService : ISuggestedUsersService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRequestRepository _connRequestsRepo;
        private readonly IConnectionRepository _connsRepo;

        public RandomSuggestedUsersService(IUserRepository userRepo/*,IConnectionRepository connsRepo, IRequestRepository connRequestsRepo*/)
        {
            _userRepo = userRepo;
            //_connsRepo = connsRepo;
            //_connRequestsRepo = connRequestsRepo;
        }

        public async Task<List<UserSuggestedDto>> GetSuggestedUsers(UserId userId, int numberOfSuggestions)
        {
            if (_userRepo.GetByIdAsync(userId) == null)
            {
                return null;
            }

            //var pendingUserRequests = await _connRequestsRepo.GetUsersFromPendingRequests(userId);
            //var friends = await _connsRepo.GetUserFriends(userId);
            //var users=await _userRepo.GetAllNonFriendsAndWithoutFriendRequests(userId,pendingUserRequests,friends);
            var users = await _userRepo.GetAllNonFriendsAndWithoutFriendRequests(userId);
            if (users == null)
            {
                return null;
            }
            if (users.Count==0)
            {
                return new List<UserSuggestedDto>(); //empty list
            }
            
            var r = new Random(users.Count);
            var indices = new HashSet<int>();
            var usersDto = new List<UserSuggestedDto>();
            for (var i = 0; i< users.Count && i < numberOfSuggestions;)
            {
                var index = r.Next(users.Count - i);
                if (indices.Contains(index))
                {
                    continue;
                }
                indices.Add(index);
                usersDto.Add(users[index].toUserSuggestedDto());
                i++;
            }
            return usersDto;
        }
    }
}