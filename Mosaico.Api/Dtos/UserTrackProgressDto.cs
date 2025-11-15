namespace Mosaico.Api.Dtos
{
    public class UserTrackProgressDto
    {
        public int UserId { get; set; }
        public int TrackId { get; set; }
        public string TrackTitle { get; set; } = default!;
        public int LessonsCompleted { get; set; }
        public int ProgressPercentage { get; set; }
    }

    public class UpdateProgressRequestDto
    {
        public int LessonsCompleted { get; set; }
    }
}
