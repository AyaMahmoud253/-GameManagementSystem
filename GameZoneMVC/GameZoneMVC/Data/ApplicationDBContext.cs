using Microsoft.EntityFrameworkCore;
using GameZoneMVC.Models;

namespace GameZoneMVC.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }

        // Seeding data into the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding categories
            modelBuilder.Entity<Category>()
                .HasData(new Category[]
                {
                    new Category { Id = 1, Name = "Sports" },
                    new Category { Id = 2, Name = "Action" },
                    new Category { Id = 3, Name = "Adventure" },
                    new Category { Id = 4, Name = "Racing" },
                    new Category { Id = 5, Name = "Fighting" },
                    new Category { Id = 6, Name = "Film" }
                });

            // Seeding devices
            modelBuilder.Entity<Device>()
                .HasData(new Device[]
                {
                    new Device { Id = 1, Name = "PlayStation", Icon = "bi bi-playstation" },
                    new Device { Id = 2, Name = "Xbox", Icon = "bi bi-xbox" },
                    new Device { Id = 3, Name = "Nintendo Switch", Icon = "bi bi-nintendo-switch" },
                    new Device { Id = 4, Name = "PC", Icon = "bi bi-pc-display" }
                });

            // Configuring many-to-many relationship for Game and Device
            modelBuilder.Entity<GameDevice>()
                .HasKey(e => new { e.GameId, e.DeviceId });

            // Example seeding for Games
            modelBuilder.Entity<Game>()
                .HasData(new Game[]
                {
                    new Game { Id = 1, Name = "FIFA 2024", CategoryId = 1, Description = "A popular football game", Cover = "fifa2024.jpg" },
                    new Game { Id = 2, Name = "Call of Duty", CategoryId = 2, Description = "First-person shooter game", Cover = "cod.jpg" }
                });

            // Example seeding for GameDevice (linking Games with Devices)
            modelBuilder.Entity<GameDevice>()
                .HasData(new GameDevice[]
                {
                    new GameDevice { GameId = 1, DeviceId = 1 }, // FIFA 2024 on PlayStation
                    new GameDevice { GameId = 1, DeviceId = 4 }, // FIFA 2024 on PC
                    new GameDevice { GameId = 2, DeviceId = 2 }, // Call of Duty on Xbox
                    new GameDevice { GameId = 2, DeviceId = 4 }  // Call of Duty on PC
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
