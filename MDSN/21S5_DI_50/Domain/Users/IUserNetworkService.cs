using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDSample1.Domain.Users
{
    public interface IUserNetworkService
    {
        Task<UserNetworkDTO> GetUserNetwork(UserId id, int level);
        Task<UserNetworkOperationsDTO> GetUserNetworkSize(UserId id, int level);
        Task<UserNetworkOperationsDTO> GetNetworkDimension();
        Task<int> GetNetworkConnectionStrength(Guid id);
        Task<List<UserDto>> GetCommonFriends(UserId user1, UserId user2);
        Task<List<String>> getGroupSuggestions(GetGroupSuggestionsDTO dto);
    }
}