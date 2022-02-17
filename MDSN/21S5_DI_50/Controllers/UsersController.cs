using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IUserNetworkService _networkService;

        public UsersController(IUserService service, IUserNetworkService networkService)
        {
            _service = service;
            this._networkService = networkService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return await _service.GetAllAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await _service.GetByIdAsync(new UserId(id));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {

            try{
                var userId = new UserId(id);
                var deletedUser = await _service.DeleteAsync(userId);
                if(deletedUser == null){
                    return BadRequest("User does not exist");
                }
                return Ok(deletedUser);
            }catch(BusinessRuleValidationException ex){
                 return BadRequest(new {ex.Message});
            }

        }

        // GET: api/Users/mutualFriends/5/6
        [HttpGet("mutualFriends/{OrigUserId}/{DestUserId}")]
        public async Task<IActionResult> getListOfMutualFriends(Guid OrigUserId,Guid DestUserId)
        {   
            try{
                var MutualFriends = await _service.getListOfMutualFriends(new UserId(OrigUserId),new UserId(DestUserId));

                if(MutualFriends == null) return NotFound();

                return Ok(MutualFriends);

            }catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserPrivateProfile(Guid id)
        {
            try
            {
                var user = await _service.GetPrivateProfileByIdAsync(new UserId(id));

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            } catch (BusinessRuleValidationException)
            {
                return BadRequest(id);
            }
        }

        [HttpGet("profile/email/{email}")]
        public async Task<IActionResult> GetUserPrivateProfileByEmail(string email)
        {
            try
            {
                var user = await _service.GetPrivateProfileByEmailAsync(new Email(email));

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            } catch (BusinessRuleValidationException)
            {
                return BadRequest(email);
            }
        }
        
        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdatePrivateProfile(Guid id, UserPrivateProfileDto dto)
        {
            try
            {
                var user = await _service.UpdatePrivateProfileAsync(dto);
                
                if(user == null)
                    return NotFound();

                return Ok(user);    
            }catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        // GET: api/Users/network/5
        [HttpGet("network/{id}/{level}")]
        public async Task<ActionResult<UserNetworkDTO>> GetUserNetwork(Guid id, int level)
        {
            var network = await this._networkService.GetUserNetwork(new UserId(id), level);
            if (network == null)
            {
                return NotFound();
            }
            return network;
        }
        // GET: api/Users/suggested/3
        [HttpGet("suggested/{id}")]
        public async Task<IActionResult> GetUserSuggestedPreviewById(Guid id)
        {
            var user = await _service.GetBySuggestionIdAsync(new UserId(id));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserDto dto)
        {
           

            try{
                var user = await _service.UpdateAsync(dto);

                if(user == null)
                    return NotFound();

                return Ok(user);    
            }catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPatch("emotionalState/{id}")]
        public async Task<IActionResult> UpdateUserEmotionalState(Guid id,UserUpdateEmotionalStateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            try
            {
                var user = await _service.UpdateEmotionalState(dto);

                if(user == null)
                    return NotFound();

                return Ok(user);    
            }
            catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser(CreatingUserDto userData)
        {
            try{
                var user = await _service.RegisterUser(userData);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }catch(DbUpdateException){
                return BadRequest("User already registered");
            }
            
           
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto loginData)
        {
            var user = await _service.LoginUser(loginData);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }
        [HttpGet("commonFriends/{userId1:guid}/{userId2:guid}")]
        public async Task<IActionResult> GetCommonFriends(Guid userId1, Guid userId2)
        {
            try
            {
                var user1 = new UserId(userId1);
                var user2 = new UserId(userId2);
                var commonFriends = await _networkService.GetCommonFriends(user1, user2);
                if (commonFriends == null)
                {
                    return NotFound("User(s) not found");
                }
                return Ok(commonFriends);
            }catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
        
        

    }
}
