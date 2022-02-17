using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Connections;
using DDDSample1.Infrastructure;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;

namespace DDDNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly IConnectionService _service;

        public ConnectionsController(IConnectionService service)
        {
            _service = service;
        }

        // GET: api/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetConnections()
        {
            return  Ok(await _service.GetAllConnections());
        }

        // GET: api/bidirecional
        [HttpGet("bidirecional")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllBidirecionalConnections()
        {

            try{
                    var biConnections = await _service.GetAllBidirectionalConnections();
                    
                    return Ok(biConnections);

            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }


        // GET: api/Connections/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<UserConnectionsDTO>> GetConnectionsOfUser(Guid userId)
        {
            try
            {
                var connections = await _service.GetConnectionsOfUser(new UserId(userId));

                if (connections == null)
                {
                    return NotFound();
                }

                return connections;
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        // GET: api/Connections/destinyUsers/{userId}
        [HttpGet("destinyUsers/{userId}")]
        public async Task<IActionResult> getPossibleDestinyUsers(Guid userId)
        {
            try{

                var users = await _service.GetPossibleDestinyUsers(new UserId(userId));

                if(users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch(BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        // PATCH: api/Connections/{connId}
        [HttpPatch("{connId}")]
        public async Task<ActionResult<ConnectionDTO>> ChangeConnectionInfo(Guid connId, ChangeConnInfoDTO connInfo)
        {
            var conn = await _service.UpdateConnection(new ConnectionId(connId), connInfo);
            
            if (conn == null)
            {
                return NotFound();
            }

            return Ok(conn);
        }

        /*
        // GET: api/Connections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Connection>>> GetConnections()
        {
            return await _context.Connections.ToListAsync();
        }

        // GET: api/Connections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Connection>> GetConnection(ConnectionId id)
        {
            var connection = await _context.Connections.FindAsync(id);

            if (connection == null)
            {
                return NotFound();
            }

            return connection;
        }

        // PUT: api/Connections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConnection(ConnectionId id, Connection connection)
        {
            if (id != connection.Id)
            {
                return BadRequest();
            }

            _context.Entry(connection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConnectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Connections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Connection>> PostConnection(Connection connection)
        {
            _context.Connections.Add(connection);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ConnectionExists(connection.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetConnection", new { id = connection.Id }, connection);
        }

        // DELETE: api/Connections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConnection(ConnectionId id)
        {
            var connection = await _context.Connections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }

            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConnectionExists(ConnectionId id)
        {
            return _context.Connections.Any(e => e.Id == id);
        }
        */
        
        [HttpGet("strengths/{connectionId}")]
        public async Task<IActionResult> GetStrengthOfConnection(Guid connectionId)
        {
            try
            {
                var connections = await _service.GetStrengthOfConnection(new ConnectionId(connectionId));

                if (connections == null)
                {
                    return NotFound();
                }

                return Ok(connections);
            }
            catch (BusinessRuleValidationException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

    }
}
