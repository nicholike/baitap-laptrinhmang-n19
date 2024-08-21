using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Xml;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {

        private readonly ApplicationDbContext _context;


        private readonly ILogger<InfoController> _logger;

        public InfoController(ILogger<InfoController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InfoEntity>>> GetMyEntities()
        {
            return await _context.MyEntities.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<InfoEntity>> PostMyEntity(InfoEntity entity)
        {
            _context.MyEntities.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyEntities), new { id = entity.Id }, entity);
        }
    }
}