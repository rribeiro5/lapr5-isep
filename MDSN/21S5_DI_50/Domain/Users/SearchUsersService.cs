using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Utils;

namespace DDDSample1.Domain.Users
{
    public class SearchUsersService : ISearchUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        
        public SearchUsersService(IUnitOfWork unitOfWork, IUserRepository userRepo)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            
        }

        public async Task<List<UserSearchedDTO>> GetUserByName(string name)
        {
            var users = await _userRepo.GetUserByName(name);
            if (users.IsNullOrEmpty()) return new List<UserSearchedDTO>();
            var usersDto = new List<UserSearchedDTO>();
            foreach (var user in users)
            {
                usersDto.Add(user.toUserSearchedDTO());
            }

            return usersDto;

        }

        public async Task<UserSearchedDTO> GetUserByEmail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            return user.toUserSearchedDTO();
        }

        public async Task<List<UserSearchedDTO>> GetUsersByTags(IList<string> tags)
        {
            var users = await _userRepo.GetUsersByTags(tags);
            if (users.IsNullOrEmpty()) return new List<UserSearchedDTO>();
            var usersDto = new List<UserSearchedDTO>();
            foreach (var user in users)
            {
                usersDto.Add(user.toUserSearchedDTO());
            }

            return usersDto;
        }

        public async Task<List<UserSearchedDTO>> GetUsersByCountry(string country)
        {
            var users = await _userRepo.GetUsersByCountry(country);
            if (users.IsNullOrEmpty()) return new List<UserSearchedDTO>();
            var usersDto = new List<UserSearchedDTO>();
            foreach (var user in users)
            {
                usersDto.Add(user.toUserSearchedDTO());
            }

            return usersDto;
        }
    }
}