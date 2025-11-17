using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Dtos;
using Mosaico.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;


namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users/{userId:int}/missions")]
    [Authorize]
    public class UserMissionsController : ControllerBase
    {
        private readonly MosaicoContext _context;

        public UserMissionsController(MosaicoContext context)
        {
            _context = context;
        }

        // GET: api/v1/users/1/missions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMissionDto>>> GetUserMissions(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return NotFound($"Usuário {userId} não encontrado.");

            var missions = await _context.UsersMissions
                .Include(um => um.Mission)
                .Where(um => um.UserId == userId)
                .Select(um => new UserMissionDto
                {
                    Id = um.Id,
                    UserId = um.UserId,
                    MissionId = um.MissionId,
                    MissionTitle = um.Mission.Title,
                    IsCompleted = um.IsCompleted,
                    CompletedAt = um.CompletedAt
                })
                .ToListAsync();

            return Ok(missions);
        }

        // POST: api/v1/users/1/missions/2/complete
        [HttpPost("{missionId:int}/complete")]
        public async Task<ActionResult<UserMissionDto>> CompleteMission(int userId, int missionId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"Usuário {userId} não encontrado.");

            var mission = await _context.Missions.FindAsync(missionId);
            if (mission == null)
                return NotFound($"Missão {missionId} não encontrada.");

            var userMission = await _context.UsersMissions
                .FirstOrDefaultAsync(um => um.UserId == userId && um.MissionId == missionId);

            if (userMission == null)
            {
                userMission = new UserMission
                {
                    UserId = userId,
                    MissionId = missionId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow
                };
                _context.UsersMissions.Add(userMission);
            }
            else
            {
                if (userMission.IsCompleted)
                {
                    // já concluída
                    return BadRequest("Missão já foi concluída anteriormente.");
                }

                userMission.IsCompleted = true;
                userMission.CompletedAt = DateTime.UtcNow;
            }

            // aplica XP da missão
            user.Xp += mission.RewardXp;

            await _context.SaveChangesAsync();

            var dto = new UserMissionDto
            {
                Id = userMission.Id,
                UserId = userMission.UserId,
                MissionId = userMission.MissionId,
                MissionTitle = mission.Title,
                IsCompleted = userMission.IsCompleted,
                CompletedAt = userMission.CompletedAt
            };

            return Ok(dto);
        }
    }
}
