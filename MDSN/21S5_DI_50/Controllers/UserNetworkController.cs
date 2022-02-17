using System;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using DDDSample1.Domain.Users;


namespace DDDNetCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserNetworkController : ControllerBase
    {
        private readonly IUserNetworkService _service;

        public UserNetworkController(IUserNetworkService service)
        {
            _service = service;
        }
      
        
        [HttpGet("networkSize/{id}/{level}")]
        public async Task<IActionResult> GetUserNetworkSize(Guid id, int level)
        {
            try
            {
                var network = await this._service.GetUserNetworkSize(new UserId(id), level);
                if (network == null)
                {
                    return NotFound();
                }
                return Ok(network);
            } catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
        
        [HttpGet("networkDimension")]
        public async Task<IActionResult> GetNetworkDimension()
        {
            try
            {
                var network = await _service.GetNetworkDimension();

                if (network == null)
                {
                    return NotFound();
                }

                return Ok(network);
            } catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
        //todo considerar mudar para users controller
        [HttpGet("NetworkConnectionStrength/{id}")]
        public async Task<IActionResult> GetNetworkConnectionStrength(Guid id)
        {
            try
            {
                var value = await _service.GetNetworkConnectionStrength(id);
                if (value == -1)
                {
                    return NotFound("User doesn't exist");
                }
                var dto = new UserNetworkOperationsDTO(value);
                return Ok(dto);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new {e.Message});
            }
        }
    
        [HttpPost("GroupSuggestions")]
        public async Task<IActionResult> GetGroupSuggestions(GetGroupSuggestionsDTO request)
        {
            try
            {
                var result = await _service.getGroupSuggestions(request);
                if (result == null)
                {
                    return NotFound("List does not exist");
                }
                
                return Ok(result);
            }
            catch (BusinessRuleValidationException e)
            {
                return BadRequest(new {e.Message});
            }
        }
                
    }
}