using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionRequestController : ControllerBase
    {
        private readonly IConnectionRequestService _service;

       // private readonly UserService _UserService;

        public ConnectionRequestController(IConnectionRequestService service)
        {
            _service = service;
        }
        
        // GET: api/ConnectionRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectionRequestDTO>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        } 
        
        // GET: api/ConnectionRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConnectionRequestDTO>> GetRequestById(Guid id)
        {
            var request = await _service.GetByIdAsync(new RequestId(id));

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        
        // GET: api/ConnectionRequest/user/{id}
        [HttpGet("user/{id}")]
        public async Task<IActionResult> getPendingConnectionsRequestOfUser(Guid id)
        {   
            try {
                var prod = await _service.getPendingConnectionsRequestOfUser(new UserId(id));
                if(prod == null) return BadRequest();
                return Ok(prod);    
            }catch(BusinessRuleValidationException ex){
                return BadRequest(ex.Message);
            }
        
        }
        
        // GET: api/ConnectionRequest/
        [HttpGet("pendingApproval/{id}")]
        public async Task<IActionResult> GetPendingApprovalRequestsOfUser(Guid id)
        {   
            try {
                var prod = await _service.GetPendingApprovalRequestsOfUser(new UserId(id));
                if(prod == null) return BadRequest();
                return Ok(prod);    
            }catch(BusinessRuleValidationException ex){
                return BadRequest(ex.Message);
            }
        
        }
        
        
        
        [HttpPost("directConnection")]
        public async Task<IActionResult> SendConnectionRequest(CreatingRequestDTO dto)
        {
            try
            {
                var request = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetRequestById), new {id = request.Id}, request);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }


        //Post : api/ConnectionRequest/introductionRequest
        [HttpPost("introductionRequest")]
        public async Task<IActionResult> CreateIntroductionRequest(CreatingIntroductionRequestDTO dto)
        {   
            try{
                var introductionRequest = await _service.CreateIntroductionRequest(dto);
                if(introductionRequest == null) return BadRequest();
                return CreatedAtAction(nameof(GetRequestById), new {id = introductionRequest.Id}, introductionRequest);
            }
            catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
            
        }

        
        [HttpPatch("approval/{id}")]
        public async Task<IActionResult> UpdateApprovalState(UpdatedApprovalStateRequestDTO dto)
        {
           

            try{
                var c = await _service.UpdateApprovalState(dto);

                if(c == null)
                    return NotFound();
                
                return Ok(c);    
            }
            catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }

        }
        
        [HttpGet("acceptance/{userId}")]
        public async Task<ActionResult<UserRequestsDTO>> GetRequestsInAcceptance(Guid userId)
        {
            var reqs = await _service.GetRequestsInAcceptance(new UserId(userId));

            if (reqs == null)
            {
                return NotFound();
            }

            return reqs;
        }

        [HttpPatch("acceptance/{reqId}")]
        public async Task<ActionResult<ResultConnectionDTO>> RequestAcceptance(Guid reqId, RequestAcceptanceDTO answer)
        {
            try
            {
                var res = await _service.RequestAcceptance(new RequestId(reqId), answer);
            
                if (res == null)
                {
                    return NotFound();
                }

                return res;
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
    }
}