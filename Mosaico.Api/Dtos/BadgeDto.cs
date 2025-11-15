namespace Mosaico.Api.Dtos
{
    public class BadgeDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;      // ex: MOSA_FIRST_TRACK
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int UserId { get; set; }
    }
}
