using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Infrastructure.Data;

namespace Mosaico.Api.Application.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly MosaicoContext _context;

        public BadgeService(MosaicoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BadgeDto>> GetForUserAsync(int userId)
        {
            var badges = await _context.Badges
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return badges.Select(b => new BadgeDto
            {
                Id = b.Id,
                Code = b.Code,
                Name = b.Name,
                Description = b.Description,
                UserId = b.UserId
            });
        }

        public async Task<BadgeDto> GrantToUserAsync(int userId, BadgeDto dto)
        {
            var entity = new Badge
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };

            _context.Badges.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.UserId = userId;
            return dto;
        }

        public async Task RevokeAsync(int badgeId)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(b => b.Id == badgeId);
            if (badge == null)
                throw new KeyNotFoundException("Badge não encontrada.");

            _context.Badges.Remove(badge);
            await _context.SaveChangesAsync();
        }
    }
}
