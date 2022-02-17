using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly DDDSample1DbContext _context;
        private readonly IUnitOfWork _unit;

        public TestsController(DDDSample1DbContext context, IUnitOfWork unit)
        {
            _context = context;
            _unit = unit;
        }

        public ActionResult<string> DeleteAll()
        {
            _context.Database.EnsureDeleted();
            return Ok("All data were removed");
        }
        [HttpPut]
        public ActionResult AddConnection(ConnectionDTO dto)
        {
            var tags = dto.Tags.ToList();
            var c = new Connection(dto.ConnectionStrength,dto.RelationshipStrength,tags,dto.OUser,dto.DUser);
            _context.Connections.Add(c);
            _unit.CommitAsync();
            return Ok(c);
        } 
    }
}