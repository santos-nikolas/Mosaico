namespace Mosaico.Api.Dtos
{
    public class UserMissionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MissionId { get; set; }
        public string MissionTitle { get; set; } = default!;
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
