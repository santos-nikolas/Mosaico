namespace Mosaico.Api.Domain.Entities
{
    public class UserMission
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public int MissionId { get; set; }
        public Mission Mission { get; set; } = default!;

        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
