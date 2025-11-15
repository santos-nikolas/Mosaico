using System.Security;

namespace Mosaico.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string AreaOfInterest { get; set; } = default!;
        public int Level { get; set; } = 1;
        public int Xp { get; set; } = 0;

        public ICollection<UserTrackProgress> TracksProgress { get; set; } = new List<UserTrackProgress>();
        public ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
        public ICollection<Badge> Badges { get; set; } = new List<Badge>();
    }
}
