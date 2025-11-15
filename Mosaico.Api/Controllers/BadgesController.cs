using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Dtos;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users/{userId:int}/[controller]")]
    public class BadgesController : ControllerBase
    {
        private readonly MosaicoContext _context;

        public BadgesController(MosaicoContext context)
        {
            _context = context;
        }

        // GET: api/v1/users/1/badges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadgeDto>>> GetUserBadges(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return NotFound($"Usuário {userId} não encontrado.");

            var badges = await _context.Badges
                .Where(b => b.UserId == userId)
                .Select(b => new BadgeDto
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Description = b.Description,
                    UserId = b.UserId
                })
                .ToListAsync();

            return Ok(badges);
        }

        // POST: api/v1/users/1/badges
        [HttpPost]
        public async Task<ActionResult<BadgeDto>> CreateBadge(int userId, [FromBody] BadgeDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"Usuário {userId} não encontrado.");

            if (string.IsNullOrWhiteSpace(request.Code))
                return BadRequest("Code é obrigatório.");

            var badge = new Badge
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                UserId = userId
            };

            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();

            var dto = new BadgeDto
            {
                Id = badge.Id,
                Code = badge.Code,
                Name = badge.Name,
                Description = badge.Description,
                UserId = badge.UserId
            };

            return CreatedAtAction(nameof(GetUserBadges), new { userId = userId }, dto);
        }
    }
}
