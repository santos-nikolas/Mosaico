using System.Security;

namespace Mosaico.Api.Domain.Entities
{
    public class Mission
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Type { get; set; } = default!; // daily / weekly
        public int RewardXp { get; set; }

        public ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
    }
}
