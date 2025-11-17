using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users/{userId:int}/[controller]")]
    [Authorize]
    public class BadgesController : ControllerBase
    {
        private readonly IBadgeService _badgeService;

        public BadgesController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadgeDto>>> GetBadges(int userId)
        {
            return Ok(await _badgeService.GetForUserAsync(userId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BadgeDto>> GrantBadge(
            int userId,
            [FromBody] BadgeDto dto)
        {
            var created = await _badgeService.GrantToUserAsync(userId, dto);

            return CreatedAtAction(
                nameof(GetBadges),
                new { userId = userId },
                created
            );
        }

        [HttpDelete("{badgeId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RevokeBadge(int userId, int badgeId)
        {
            await _badgeService.RevokeAsync(badgeId);
            return NoContent();
        }
    }
}
