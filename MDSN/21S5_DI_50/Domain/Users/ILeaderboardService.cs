using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDSample1.Domain.Users
{
    public interface ILeaderboardService
    {
        Task<SortedSet<UserLeaderboardDTO>> GetNetworkStrengthLeaderboard();
        Task<List<UserLeaderboardDTO>> GetLeaderboardDimensionCriteria();
    }
}