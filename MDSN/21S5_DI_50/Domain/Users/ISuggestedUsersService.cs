using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Domain.Users
{
    public interface ISuggestedUsersService
    {
        public Task<List<UserSuggestedDto>> GetSuggestedUsers(UserId userId, int numberOfSuggestions);
    }
}