using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure;
using DDDSample1.Utils;
using Microsoft.Extensions.Configuration;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestUsersController : ControllerBase
    {
        private readonly ISuggestedUsersService _service;
        private readonly IConfiguration _config;
        private const int DEFAULT_NUMBER_SUGGESTIONS = 3;

        public SuggestUsersController(ISuggestedUsersService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriendSuggestions(Guid id)
        {
            try
            {
                var s = _config.GetSection("FriendSuggestionsAlgorithm")
                    .GetSection("NumberOfSuggestions")
                    .Value;
                if (!int.TryParse(s,out var n))
                {
                    n = DEFAULT_NUMBER_SUGGESTIONS;
                }
                var uId = new UserId(id);
                var l= await _service.GetSuggestedUsers(uId, n);
                if (l ==null)
                {
                    return NotFound(id);
                }
                return Ok(l);
            }
            catch (BusinessRuleValidationException)
            {
                return BadRequest(id);
            }
            
        }
    }
}
