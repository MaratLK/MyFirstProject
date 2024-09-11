using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLK_TwoTry_Back.Models;
using PLKTransit.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PLKTransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Защищаем все маршруты, доступ только для авторизованных пользователей
    public class OrdersController : ControllerBase
    {
        private readonly PLKTransitContext _context;

        public OrdersController(PLKTransitContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound($"Заказ с ID {id} не найден.");
            }

            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Orders>> PostOrder(Orders order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Получаем ID текущего пользователя из JWT-токена
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            order.UserID = userId;
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderID }, order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Orders order)
        {
            if (id != order.OrderID)
            {
                return BadRequest("ID заказа не совпадает.");
            }

            // Проверка прав: только создатель заказа может его редактировать
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (order.UserID != userId)
            {
                return Forbid("У вас нет прав для изменения этого заказа.");
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound($"Заказ с ID {id} не найден.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound($"Заказ с ID {id} не найден.");
            }

            // Проверка прав: только создатель заказа может его удалить
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (order.UserID != userId)
            {
                return Forbid("У вас нет прав для удаления этого заказа.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
