using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Domain.Users
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllAsync();

        public Task<UserDto> GetByIdAsync(UserId id);
        
        public Task<UserPrivateProfileDto> GetPrivateProfileByIdAsync(UserId id);

        public Task<UserPrivateProfileDto> GetPrivateProfileByEmailAsync(Email email);
        
        public Task<UserPrivateProfileDto> UpdatePrivateProfileAsync(UserPrivateProfileDto dto);

        public Task<List<UserDto>> getListOfMutualFriends(UserId origUserId, UserId destUserId);
        
        public Task<UserDto> RegisterUser(CreatingUserDto dto);

        public Task<UserDto> UpdateAsync(UserDto dto);
        
        public Task<UserDto> UpdateEmotionalState(UserUpdateEmotionalStateDTO dto);

        public Task<UserDto> DeleteAsync(UserId id);

        public Task<UserDto> LoginUser(LoginDto loginData);
        public Task<UserSuggestedDto> GetBySuggestionIdAsync(UserId userId);
    }
}