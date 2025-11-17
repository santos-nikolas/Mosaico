using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class MissionsController : ControllerBase
    {
        private readonly IMissionService _missionService;

        public MissionsController(IMissionService missionService)
        {
            _missionService = missionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MissionDto>>> GetMissions()
        {
            var missions = await _missionService.GetAllAsync();
            return Ok(missions);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MissionDto>> GetMission(int id)
        {
            var mission = await _missionService.GetByIdAsync(id);
            if (mission == null)
                return NotFound();

            return Ok(mission);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MissionDto>> CreateMission([FromBody] MissionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _missionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetMission), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMission(int id, [FromBody] MissionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _missionService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMission(int id)
        {
            await _missionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
