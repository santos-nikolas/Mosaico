using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Dtos;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TracksController : ControllerBase
    {
        private readonly MosaicoContext _context;

        public TracksController(MosaicoContext context)
        {
            _context = context;
        }

        // GET: api/v1/tracks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetTracks()
        {
            var tracks = await _context.Tracks
                .Select(t => new TrackDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Area = t.Area,
                    TotalLessons = t.TotalLessons,
                    EstimatedHours = t.EstimatedHours
                })
                .ToListAsync();

            return Ok(tracks);
        }

        // GET: api/v1/tracks/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrackDto>> GetTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
                return NotFound();

            var dto = new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Area = track.Area,
                TotalLessons = track.TotalLessons,
                EstimatedHours = track.EstimatedHours
            };

            return Ok(dto);
        }

        // POST: api/v1/tracks
        [HttpPost]
        public async Task<ActionResult<TrackDto>> CreateTrack([FromBody] TrackDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var track = new Track
            {
                Title = request.Title,
                Area = request.Area,
                TotalLessons = request.TotalLessons,
                EstimatedHours = request.EstimatedHours
            };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            var dto = new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Area = track.Area,
                TotalLessons = track.TotalLessons,
                EstimatedHours = track.EstimatedHours
            };

            return CreatedAtAction(nameof(GetTrack), new { id = track.Id }, dto);
        }

        // PUT: api/v1/tracks/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTrack(int id, [FromBody] TrackDto request)
        {
            if (id != request.Id)
                return BadRequest("Id do corpo difere do id da rota.");

            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
                return NotFound();

            track.Title = request.Title;
            track.Area = request.Area;
            track.TotalLessons = request.TotalLessons;
            track.EstimatedHours = request.EstimatedHours;

            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // DELETE: api/v1/tracks/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
                return NotFound();

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}
