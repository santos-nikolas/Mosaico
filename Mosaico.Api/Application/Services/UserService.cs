using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Dtos;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Infrastructure.Data;
using Mosaico.Api.Enums;

namespace Mosaico.Api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly MosaicoContext _context;

        public UserService(MosaicoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Username = u.Username,
                AreaOfInterest = u.AreaOfInterest,
                Level = u.Level,
                Xp = u.Xp,
                Role = u.Role.ToString()
            });
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                AreaOfInterest = user.AreaOfInterest,
                Level = user.Level,
                Xp = user.Xp,
                Role = user.Role.ToString()
            };
        }

        public async Task<UserDto> CreateAsync(UserDto dto)
        {
            // Em muitos cenários reais, você usaria a lógica do AuthService aqui.
            // Pra GS, pode nem usar esse método. Mas deixo como exemplo:

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Username = dto.Username,
                AreaOfInterest = dto.AreaOfInterest,
                Level = dto.Level,
                Xp = dto.Xp,
                Role = Enum.TryParse<UserRole>(dto.Role, true, out var role)
                    ? role
                    : UserRole.Student
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            dto.Id = user.Id;
            dto.Role = user.Role.ToString();
            return dto;
        }

        public async Task UpdateAsync(int id, UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Username = dto.Username;
            user.AreaOfInterest = dto.AreaOfInterest;
            user.Level = dto.Level;
            user.Xp = dto.Xp;

            if (Enum.TryParse<UserRole>(dto.Role, true, out var role))
            {
                user.Role = role;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
