using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Xml;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeController : ControllerBase
    {

        private readonly CodeDbContext _context;


        private readonly ILogger<CodeController> _logger;

        public CodeController(ILogger<CodeController> logger, CodeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeEntity>>> GetMyEntities()
        {
            return await _context.Codes.ToListAsync();
        }
    }
}