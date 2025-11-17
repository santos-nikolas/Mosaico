using Mosaico.Api.Dtos;

namespace Mosaico.Api.Application.Interfaces
{
    public interface ITrackService
    {
        Task<IEnumerable<TrackDto>> GetAllAsync();
        Task<TrackDto?> GetByIdAsync(int id);
        Task<TrackDto> CreateAsync(TrackDto dto);
        Task UpdateAsync(int id, TrackDto dto);
        Task DeleteAsync(int id);
    }
}
