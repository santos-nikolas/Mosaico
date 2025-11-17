using System.ComponentModel.DataAnnotations;

namespace Mosaico.Api.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string AreaOfInterest { get; set; } = default!;

        // Campo obrigatório: Student, Company ou Admin
        [Required]
        public string Role { get; set; } = default!;
    }
}

