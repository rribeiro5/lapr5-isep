
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController: ControllerBase
    {
        private readonly ILeaderboardService _service;


        public LeaderboardController(ILeaderboardService service)
        {
            _service = service;
        }
        
        [HttpGet("networkDimension")]
        public async Task<IActionResult> GetLeaderboardDimensionCriteria()
        {
            try
            {
                var network = await _service.GetLeaderboardDimensionCriteria();

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

        [HttpGet("networkStrength")]
        public async Task<IActionResult> GetNetworkStrengthLeaderboard()
        {
            try
            {
                var userLeaderboard = await _service.GetNetworkStrengthLeaderboard();
                if (userLeaderboard == null)
                {
                    return NotFound();
                }
                return Ok(userLeaderboard);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
    }
}