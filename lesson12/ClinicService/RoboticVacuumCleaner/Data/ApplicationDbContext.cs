using Microsoft.EntityFrameworkCore;
using RoboticVacuumCleaner.Server.Models;
using RoboticVacuumCleaner.Server.Models;
using System.Reflection.Emit;

namespace RoboticVacuumCleaner.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Robot> Robots { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<CleaningSession> CleaningSessions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
            });

            // Robot configuration
            modelBuilder.Entity<Robot>(entity =>
            {
                entity.HasIndex(r => r.SerialNumber).IsUnique();
                entity.HasIndex(r => r.MacAddress).IsUnique();
                entity.Property(r => r.SerialNumber).IsRequired().HasMaxLength(50);
                entity.Property(r => r.MacAddress).IsRequired().HasMaxLength(17);
                entity.Property(r => r.Model).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Name).HasMaxLength(100);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Robots)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Room configuration
            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Coordinates).HasColumnType("TEXT");
                entity.Property(r => r.NoGoZones).HasColumnType("TEXT");

                entity.HasOne(r => r.Robot)
                    .WithMany(r => r.Rooms)
                    .HasForeignKey(r => r.RobotId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CleaningSession configuration
            modelBuilder.Entity<CleaningSession>(entity =>
            {
                entity.Property(c => c.RoomsCleaned).HasColumnType("TEXT");
                entity.Property(c => c.PathHistory).HasColumnType("TEXT");
                entity.Property(c => c.CleaningMap).HasColumnType("TEXT");

                entity.HasOne(c => c.Robot)
                    .WithMany(r => r.CleaningSessions)
                    .HasForeignKey(c => c.RobotId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Schedule)
                    .WithMany(s => s.CleaningSessions)
                    .HasForeignKey(c => c.ScheduleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Schedule configuration
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(s => s.CronExpression).HasMaxLength(100);
                entity.Property(s => s.RoomsToClean).HasColumnType("TEXT");

                entity.HasOne(s => s.Robot)
                    .WithMany(r => r.Schedules)
                    .HasForeignKey(s => s.RobotId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MaintenanceRecord configuration
            modelBuilder.Entity<MaintenanceRecord>(entity =>
            {
                entity.Property(m => m.Notes).HasColumnType("TEXT");

                entity.HasOne(m => m.Robot)
                    .WithMany(r => r.MaintenanceRecords)
                    .HasForeignKey(m => m.RobotId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}