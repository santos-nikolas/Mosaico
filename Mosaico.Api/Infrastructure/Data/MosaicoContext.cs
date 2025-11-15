using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Domain.Entities;

namespace Mosaico.Api.Infrastructure.Data
{
    public class MosaicoContext : DbContext
    {
        public MosaicoContext(DbContextOptions<MosaicoContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Track> Tracks => Set<Track>();
        public DbSet<UserTrackProgress> UsersTracksProgress => Set<UserTrackProgress>();
        public DbSet<Mission> Missions => Set<Mission>();
        public DbSet<UserMission> UsersMissions => Set<UserMission>();
        public DbSet<Badge> Badges => Set<Badge>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamentos UserTrackProgress
            modelBuilder.Entity<UserTrackProgress>()
                .HasOne(utp => utp.User)
                .WithMany(u => u.TracksProgress)
                .HasForeignKey(utp => utp.UserId);

            modelBuilder.Entity<UserTrackProgress>()
                .HasOne(utp => utp.Track)
                .WithMany(t => t.UsersProgress)
                .HasForeignKey(utp => utp.TrackId);

            // Relacionamentos UserMission
            modelBuilder.Entity<UserMission>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMissions)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMission>()
                .HasOne(um => um.Mission)
                .WithMany(m => m.UserMissions)
                .HasForeignKey(um => um.MissionId);

            // Relacionamento Badge
            modelBuilder.Entity<Badge>()
                .HasOne(b => b.User)
                .WithMany(u => u.Badges)
                .HasForeignKey(b => b.UserId);
        }
    }
}
