using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Application.Services
{
    public class TrackService : ITrackService
    {
        private readonly MosaicoContext _context;

        public TrackService(MosaicoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrackDto>> GetAllAsync()
        {
            var tracks = await _context.Tracks.AsNoTracking().ToListAsync();

            return tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                Area = t.Area,
                TotalLessons = t.TotalLessons,
                EstimatedHours = t.EstimatedHours
            });
        }

        public async Task<TrackDto?> GetByIdAsync(int id)
        {
            var track = await _context.Tracks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (track == null) return null;

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                Area = track.Area,
                TotalLessons = track.TotalLessons,
                EstimatedHours = track.EstimatedHours
            };
        }

        public async Task<TrackDto> CreateAsync(TrackDto dto)
        {
            var entity = new Track
            {
                Title = dto.Title,
                Area = dto.Area,
                TotalLessons = dto.TotalLessons,
                EstimatedHours = dto.EstimatedHours
            };

            _context.Tracks.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task UpdateAsync(int id, TrackDto dto)
        {
            var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == id);
            if (track == null)
                throw new KeyNotFoundException("Trilha não encontrada.");

            track.Title = dto.Title;
            track.Area = dto.Area;
            track.TotalLessons = dto.TotalLessons;
            track.EstimatedHours = dto.EstimatedHours;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Id == id);
            if (track == null)
                throw new KeyNotFoundException("Trilha não encontrada.");

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
        }
    }
}
