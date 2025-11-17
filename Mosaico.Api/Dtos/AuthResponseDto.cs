namespace Mosaico.Api.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
