namespace Mosaico.Api.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }          // para respostas
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string AreaOfInterest { get; set; } = default!;
        public int Level { get; set; }
        public int Xp { get; set; }
    }
}
