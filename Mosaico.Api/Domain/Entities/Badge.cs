namespace Mosaico.Api.Domain.Entities
{
    public class Badge
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;      // ex: MOSA_FIRST_TRACK
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
