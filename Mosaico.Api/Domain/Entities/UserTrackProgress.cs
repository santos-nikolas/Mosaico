namespace Mosaico.Api.Domain.Entities
{
    public class UserTrackProgress
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public int TrackId { get; set; }
        public Track Track { get; set; } = default!;

        public int LessonsCompleted { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
