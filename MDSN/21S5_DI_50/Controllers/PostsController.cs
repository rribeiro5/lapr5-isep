using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DDDSample1.Domain.Posts;
using DDDSample1.Infrastructure.Posts;
using Microsoft.AspNetCore.Mvc;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        private readonly IPostsService _service;

        public PostsController(IPostsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostRequestDTO requestDto)
        {   
            try{
                var response = await _service.CreatePost(requestDto);
                if (response == null)
                {
                    return NotFound("User not found");
                }
                return Created("",response);
            }catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet("feed/{userId}")]
        public async Task<IActionResult> FeedPosts(Guid userId) {
            try {
                var response = await _service.FeedPosts(userId);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            } catch(BusinessRuleValidationException ex) {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPost("reactions")]
        public async Task<IActionResult> updateReactionPost(CreatingReactionDTO reactionDTO)
        {   
            try{
                var response = await _service.updateReactionPost(reactionDTO);
                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);

            }catch(BusinessRuleValidationException ex){
                return BadRequest(new {Message = ex.Message});
            }catch(System.FormatException ){
                return BadRequest(new {Message = "Invalid format for IDs"});
            }
            
        }

        [HttpPost("comments/reactions")]
        public async Task<IActionResult> updateReactionComment(CreatingReactionDTO reactionDTO)
        {   
            try{
                var response = await _service.updateReactionComment(reactionDTO);
                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);

            }catch(BusinessRuleValidationException ex){
                return BadRequest(new {Message = ex.Message});
            }catch(System.FormatException ){
                return BadRequest(new {Message = "Invalid format for IDs"});
            }
            
        }
        
        
        [HttpPost("comments")]
        public async Task<IActionResult> CreateComment(CreatingCommentDTO commentDTO)
        {   
            try{
                var response = await _service.CreateComment(commentDTO);
                if (response == null)
                {
                    return NotFound();
                }

                return Created("",response);

            }catch(BusinessRuleValidationException ex){
                return BadRequest(new {Message = ex.Message});
            }catch(System.FormatException ex){
                return BadRequest(new {Message = ex.Message});
            }
            
        }

    }
}