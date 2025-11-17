using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mosaico.Api.Dtos;
using Mosaico.Api.Enums;
using Mosaico.Api.Domain.Entities;
using Mosaico.Api.Settings;
using Mosaico.Api.Infrastructure.Data; // ajuste se o namespace do seu DbContext for outro
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Mosaico.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly MosaicoContext _context; // TROQUE pelo nome real do seu DbContext se for diferente
        private readonly JwtSettings _jwtSettings;

        public AuthController(MosaicoContext context, IOptions<JwtSettings> jwtOptions)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) Verificar se já existe usuário com mesmo email/username
            var exists = await _context.Users
                .AnyAsync(u => u.Email == request.Email || u.Username == request.Username);

            if (exists)
                return BadRequest("Já existe um usuário com esse email ou username.");

            // 2) Validar e converter o Role vindo da requisição (string) para enum UserRole
            if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
            {
                return BadRequest("Perfil inválido. Valores aceitos: 'Student', 'Company', 'Admin'.");
            }

            // 3) Gerar hash da senha
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 4) Criar o usuário com base no Role informado
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Username = request.Username,
                PasswordHash = passwordHash,
                AreaOfInterest = request.AreaOfInterest,
                Level = 1,
                Xp = 0,
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 5) Gerar token já logando o usuário após registro
            var token = GenerateJwtToken(user, out var expiresAt);

            var response = new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                Username = user.Username,
                Role = user.Role.ToString()
            };

            return Ok(response);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos.");

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordValid)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = GenerateJwtToken(user, out var expiresAt);

            var response = new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                Username = user.Username,
                Role = user.Role.ToString()
            };

            return Ok(response);
        }

        private string GenerateJwtToken(User user, out DateTime expiresAt)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var now = DateTime.UtcNow;
            expiresAt = now.AddMinutes(_jwtSettings.ExpiresInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
