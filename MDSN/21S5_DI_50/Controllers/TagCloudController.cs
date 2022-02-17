using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagCloudController : ControllerBase
    {
        private readonly ITagService _service;

        public TagCloudController(ITagService service)
        {
            _service = service;
        }
 
        [HttpGet("users")]
        public async Task<IActionResult> GetUsersTagCloud()
        {
            try
            {
                var tagCloud =await _service.GetUsersTagCloud();
                if (tagCloud == null)
                {
                    return NotFound("Error while processing tag cloud");
                }

                return Ok(tagCloud);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
        
        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetUserTagCloud(Guid id)
        {
            try
            {
                var tagCloud = await _service.GetUserTagCloud(id);

                if (tagCloud == null)
                {
                    return NotFound("User not found");
                }

                return Ok(tagCloud);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {ex.Message});
            }
        }
        
        [HttpGet("connection/{userId}")]
        public async Task<IActionResult> GetAllUserConnectionsTagCloud(Guid userId)
        {
            try
            {
                var network = await this._service.GetAllUserConnectionsTagCloud(userId);
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

        [HttpGet("connection")]
        public async Task<IActionResult> GetAllConnectionsTagCloud()
        {
            try
            {
                var network = await this._service.GetAllConnectionsTagCloud();
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
        
    }
}