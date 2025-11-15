using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Dtos;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MissionsController : ControllerBase
    {
        private readonly MosaicoContext _context;

        public MissionsController(MosaicoContext context)
        {
            _context = context;
        }

        // GET: api/v1/missions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MissionDto>>> GetMissions()
        {
            var missions = await _context.Missions
                .Select(m => new MissionDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Type = m.Type,
                    RewardXp = m.RewardXp
                })
                .ToListAsync();

            return Ok(missions);
        }

        // GET: api/v1/missions/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MissionDto>> GetMission(int id)
        {
            var m = await _context.Missions.FindAsync(id);
            if (m == null)
                return NotFound();

            var dto = new MissionDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Type = m.Type,
                RewardXp = m.RewardXp
            };

            return Ok(dto);
        }

        // POST: api/v1/missions
        [HttpPost]
        public async Task<ActionResult<MissionDto>> CreateMission([FromBody] MissionDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mission = new Mission
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                RewardXp = request.RewardXp
            };

            _context.Missions.Add(mission);
            await _context.SaveChangesAsync();

            var dto = new MissionDto
            {
                Id = mission.Id,
                Title = mission.Title,
                Description = mission.Description,
                Type = mission.Type,
                RewardXp = mission.RewardXp
            };

            return CreatedAtAction(nameof(GetMission), new { id = mission.Id }, dto);
        }

        // PUT: api/v1/missions/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMission(int id, [FromBody] MissionDto request)
        {
            if (id != request.Id)
                return BadRequest("Id do corpo difere do id da rota.");

            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
                return NotFound();

            mission.Title = request.Title;
            mission.Description = request.Description;
            mission.Type = request.Type;
            mission.RewardXp = request.RewardXp;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // DELETE: api/v1/missions/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMission(int id)
        {
            var mission = await _context.Missions.FindAsync(id);
            if (mission == null)
                return NotFound();

            _context.Missions.Remove(mission);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
