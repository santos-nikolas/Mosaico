namespace Mosaico.Api.Domain.Entities
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Area { get; set; } = default!;
        public int TotalLessons { get; set; }
        public int EstimatedHours { get; set; }

        public ICollection<UserTrackProgress> UsersProgress { get; set; } = new List<UserTrackProgress>();
    }
}
