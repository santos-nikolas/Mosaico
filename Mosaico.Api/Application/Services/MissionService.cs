using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Infrastructure.Data;
using Mosaico.Api.Enums;

namespace Mosaico.Api.Application.Services
{
    public class MissionService : IMissionService
    {
        private readonly MosaicoContext _context;

        public MissionService(MosaicoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MissionDto>> GetAllAsync()
        {
            var missions = await _context.Missions.AsNoTracking().ToListAsync();

            return missions.Select(m => new MissionDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Type = m.Type.ToString(),   // se o DTO usa string
                RewardXp = m.RewardXp
            });
        }

        public async Task<MissionDto?> GetByIdAsync(int id)
        {
            var mission = await _context.Missions.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null) return null;

            return new MissionDto
            {
                Id = mission.Id,
                Title = mission.Title,
                Description = mission.Description,
                Type = mission.Type.ToString(),
                RewardXp = mission.RewardXp
            };
        }

        public async Task<MissionDto> CreateAsync(MissionDto dto)
        {
            // converter string -> enum, se for o caso
            if (!Enum.TryParse<MissionType>(dto.Type, true, out var type))
            {
                throw new ArgumentException("Tipo de missão inválido.");
            }

            var entity = new Mission
            {
                Title = dto.Title,
                Description = dto.Description,
                Type = type,
                RewardXp = dto.RewardXp
            };

            _context.Missions.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.Type = entity.Type.ToString();
            return dto;
        }

        public async Task UpdateAsync(int id, MissionDto dto)
        {
            var mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null)
                throw new KeyNotFoundException("Missão não encontrada.");

            if (!Enum.TryParse<MissionType>(dto.Type, true, out var type))
            {
                throw new ArgumentException("Tipo de missão inválido.");
            }

            mission.Title = dto.Title;
            mission.Description = dto.Description;
            mission.Type = type;
            mission.RewardXp = dto.RewardXp;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mission = await _context.Missions.FirstOrDefaultAsync(m => m.Id == id);
            if (mission == null)
                throw new KeyNotFoundException("Missão não encontrada.");

            _context.Missions.Remove(mission);
            await _context.SaveChangesAsync();
        }
    }
}
