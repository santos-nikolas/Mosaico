using Mosaico.Api.Enums;

namespace Mosaico.Api.Domain.Entities
{
    public class Mission
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        // Agora usando Enum para tipo de missão
        public MissionType Type { get; set; }

        public int RewardXp { get; set; }

        public ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
    }
}
