using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLK_TwoTry_Back.Models;
using PLKTransit.Data;
using Microsoft.AspNetCore.Authorization;

namespace PLKTransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Защищаем доступ к методам сервиса, только авторизованные пользователи могут вызывать эти методы
    public class ServicesController : ControllerBase
    {
        private readonly PLKTransitContext _context;

        public ServicesController(PLKTransitContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Services>>> GetServices([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var services = await _context.Services
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(services);
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Services>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound($"Сервис с ID {id} не найден.");
            }

            return Ok(service);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<ActionResult<Services>> PostService([FromBody] Services service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = service.ServiceID }, service);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, [FromBody] Services service)
        {
            if (id != service.ServiceID)
            {
                return BadRequest("ID сервиса не совпадает.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return NotFound($"Сервис с ID {id} не найден.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound($"Сервис с ID {id} не найден.");
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceID == id);
        }
    }
}
