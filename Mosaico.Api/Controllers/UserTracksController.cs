using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Dtos;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/users/{userId:int}/tracks")]
    public class UserTracksController : ControllerBase
    {
        private readonly MosaicoContext _context;

        public UserTracksController(MosaicoContext context)
        {
            _context = context;
        }

        // GET: api/v1/users/1/tracks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTrackProgressDto>>> GetUserTracks(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return NotFound($"Usuário {userId} não encontrado.");

            var progressList = await _context.UsersTracksProgress
                .Include(utp => utp.Track)
                .Where(utp => utp.UserId == userId)
                .Select(utp => new UserTrackProgressDto
                {
                    UserId = utp.UserId,
                    TrackId = utp.TrackId,
                    TrackTitle = utp.Track.Title,
                    LessonsCompleted = utp.LessonsCompleted,
                    ProgressPercentage = utp.ProgressPercentage
                })
                .ToListAsync();

            return Ok(progressList);
        }

        // POST: api/v1/users/1/tracks/2/progress
        [HttpPost("{trackId:int}/progress")]
        public async Task<ActionResult<UserTrackProgressDto>> UpdateUserTrackProgress(
            int userId,
            int trackId,
            [FromBody] UpdateProgressRequestDto request)
        {
            if (request.LessonsCompleted < 0)
                return BadRequest("LessonsCompleted não pode ser negativo.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"Usuário {userId} não encontrado.");

            var track = await _context.Tracks.FindAsync(trackId);
            if (track == null)
                return NotFound($"Trilha {trackId} não encontrada.");

            var progress = await _context.UsersTracksProgress
                .FirstOrDefaultAsync(utp => utp.UserId == userId && utp.TrackId == trackId);

            if (progress == null)
            {
                progress = new UserTrackProgress
                {
                    UserId = userId,
                    TrackId = trackId,
                    LessonsCompleted = 0,
                    ProgressPercentage = 0
                };
                _context.UsersTracksProgress.Add(progress);
            }

            progress.LessonsCompleted = request.LessonsCompleted;

            if (track.TotalLessons > 0)
            {
                progress.ProgressPercentage = (int)(
                    (double)progress.LessonsCompleted / track.TotalLessons * 100
                );
                if (progress.ProgressPercentage > 100)
                    progress.ProgressPercentage = 100;
            }

            await _context.SaveChangesAsync();

            var dto = new UserTrackProgressDto
            {
                UserId = userId,
                TrackId = trackId,
                TrackTitle = track.Title,
                LessonsCompleted = progress.LessonsCompleted,
                ProgressPercentage = progress.ProgressPercentage
            };

            return Ok(dto);
        }
    }
}
