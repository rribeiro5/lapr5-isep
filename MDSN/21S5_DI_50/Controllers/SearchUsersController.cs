using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchUsersController : ControllerBase
    {
        private readonly ISearchUsersService _service;

        public SearchUsersController(ISearchUsersService service)
        {
            _service = service;
        }
        
        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<List<UserSearchedDTO>>> GetUserByName(string name)
        {
            return await _service.GetUserByName(name);
        }
        
        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<UserSearchedDTO>> GetUserByEmail(string email)
        {
            return await _service.GetUserByEmail(email);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<UserSearchedDTO>>> GetUsersByTags(List<string> tags)
        {
            return await _service.GetUsersByTags(tags);
        }
        
        [HttpGet("GetByCountry/{country}")]
        public async Task<ActionResult<List<UserSearchedDTO>>> GetUsersByCountry(string country)
        {
            return await _service.GetUsersByCountry(country);
        }
        
        
    }
}