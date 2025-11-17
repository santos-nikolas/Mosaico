using Mosaico.Api.Dtos;

namespace Mosaico.Api.Application.Interfaces
{
    public interface IBadgeService
    {
        Task<IEnumerable<BadgeDto>> GetForUserAsync(int userId);
        Task<BadgeDto> GrantToUserAsync(int userId, BadgeDto dto);
        Task RevokeAsync(int badgeId); // se não usar, pode tirar
    }
}
