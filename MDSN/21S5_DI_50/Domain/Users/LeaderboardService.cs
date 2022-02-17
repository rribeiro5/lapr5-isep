using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Utils;

namespace DDDSample1.Domain.Users
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IUserRepository _usersRepo;
        private readonly IConnectionRepository _connsRepo;

        public LeaderboardService(IUserRepository usersRepo, IConnectionRepository connsRepo)
        {
            _usersRepo = usersRepo;
            _connsRepo = connsRepo;
        }

        public async Task<SortedSet<UserLeaderboardDTO>> GetNetworkStrengthLeaderboard()
        {
            var comparer = LeaderboardComparer.Get();
            
            var leaderboardDtos = new SortedSet<UserLeaderboardDTO>(comparer);
            
            var userIdsStrengthsDict = await _connsRepo.GetAllUsersNetworkStrength();
            
            if (!userIdsStrengthsDict.Any())
            {
                return leaderboardDtos;
            }
            
            var users = await _usersRepo.GetByIdsAsync(userIdsStrengthsDict.Keys.ToList());
            
            foreach (var user in users)
            {
                var value = userIdsStrengthsDict[user.Id];
                var leaderboardDto = user.ToUserLeaderboardDTO(value);
                leaderboardDtos.Add(leaderboardDto);
            }
            return leaderboardDtos;
        }

        public async Task<List<UserLeaderboardDTO>> GetLeaderboardDimensionCriteria()
        {
            var lst = new List<UserLeaderboardDTO>();
            var l = await _usersRepo.GetAllAsync();
            foreach (var u in l)
            { 
               var v = await _usersRepo.GetUserFriends(u.Id);
               lst.Add(u.ToUserLeaderboardDTO(v.Count));
            }
            var sortedLst = lst.OrderByDescending(x => x.value).ToList();
            return sortedLst;
        }
    }
}