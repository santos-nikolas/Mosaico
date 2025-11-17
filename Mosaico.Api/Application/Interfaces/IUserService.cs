using Mosaico.Api.Dtos;

namespace Mosaico.Api.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(UserDto dto); // se não usar, pode tirar
        Task UpdateAsync(int id, UserDto dto); // opcional
        Task DeleteAsync(int id);
    }
}
