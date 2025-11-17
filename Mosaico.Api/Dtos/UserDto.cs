namespace Mosaico.Api.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string AreaOfInterest { get; set; } = default!;
        public int Level { get; set; }
        public int Xp { get; set; }

        // Novo campo necessário para SOA + Auth
        public string Role { get; set; } = default!;
    }
}
