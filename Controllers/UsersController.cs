    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PLK_TwoTry_Back.Models;
    using PLKTransit.Data;

    namespace PLKTransit.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class UsersController : ControllerBase
        {
            private readonly PLKTransitContext _context;

            public UsersController(PLKTransitContext context)
            {
                _context = context;
            }

            // GET: api/Users
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
            {
                return await _context.Users.Include(u => u.Role).ToListAsync();
            }

            // GET: api/Users/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Users>> GetUser(int id)
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserID == id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }

            // POST: api/Users
            [HttpPost]
            public async Task<ActionResult<Users>> PostUser(Users user)
            {
                if (user.RoleID == 0)
                {
                    var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
                    if (defaultRole != null)
                    {
                        user.RoleID = defaultRole.RoleID;
                    }
                    else
                    {
                        return BadRequest("Default role not found.");
                    }
                }

                // Установка Role после RoleID
                user.Role = await _context.Roles.FindAsync(user.RoleID);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.UserID }, user);
            }

            // PUT: api/Users/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutUser(int id, Users user)
            {
                if (id != user.UserID)
                {
                    return BadRequest();
                }

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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

            // DELETE: api/Users/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool UserExists(int id)
            {
                return _context.Users.Any(e => e.UserID == id);
            }
        }
    }
