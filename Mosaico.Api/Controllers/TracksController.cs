using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TracksController : ControllerBase
    {
        private readonly ITrackService _trackService;

        public TracksController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetTracks()
        {
            var tracks = await _trackService.GetAllAsync();
            return Ok(tracks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackDto>> GetTrack(int id)
        {
            var track = await _trackService.GetByIdAsync(id);
            if (track == null)
                return NotFound();

            return Ok(track);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TrackDto>> CreateTrack([FromBody] TrackDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _trackService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTrack), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTrack(int id, [FromBody] TrackDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _trackService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            await _trackService.DeleteAsync(id);
            return NoContent();
        }
    }
}
