using Mosaico.Api.Dtos;

namespace Mosaico.Api.Application.Interfaces
{
    public interface IMissionService
    {
        Task<IEnumerable<MissionDto>> GetAllAsync();
        Task<MissionDto?> GetByIdAsync(int id);
        Task<MissionDto> CreateAsync(MissionDto dto);
        Task UpdateAsync(int id, MissionDto dto);
        Task DeleteAsync(int id);
    }
}
