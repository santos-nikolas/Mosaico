using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Dtos;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MosaicoContext _context;

        public UsersController(MosaicoContext context)
        {
            _context = context;
        }

        // GET: api/v1/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    AreaOfInterest = u.AreaOfInterest,
                    Level = u.Level,
                    Xp = u.Xp
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/v1/users/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            var dto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                AreaOfInterest = user.AreaOfInterest,
                Level = user.Level,
                Xp = user.Xp
            };

            return Ok(dto);
        }

        // POST: api/v1/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                AreaOfInterest = request.AreaOfInterest,
                Level = request.Level == 0 ? 1 : request.Level,
                Xp = request.Xp
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                AreaOfInterest = user.AreaOfInterest,
                Level = user.Level,
                Xp = user.Xp
            };

            // 201 Created com Location
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, dto);
        }

        // PUT: api/v1/users/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto request)
        {
            if (id != request.Id)
                return BadRequest("Id do corpo difere do id da rota.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.Name = request.Name;
            user.Email = request.Email;
            user.AreaOfInterest = request.AreaOfInterest;
            user.Level = request.Level;
            user.Xp = request.Xp;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // DELETE: api/v1/users/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
