using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDSample1.Domain.Users
{
    public interface ISearchUsersService
    {
        public Task<List<UserSearchedDTO>> GetUserByName(string name);
        
        public Task<UserSearchedDTO> GetUserByEmail(string email);
        
        public Task<List<UserSearchedDTO>> GetUsersByTags(IList<string> tags);

        public Task<List<UserSearchedDTO>> GetUsersByCountry(string country);
    }
}