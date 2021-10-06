using System;
using System.Globalization;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Eventures.Data
{
    public class ApplicationDbContext : IdentityDbContext<EventuresUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
        public DbSet<Event> Events { get; set; }
        private EventuresUser GuestUser { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            SeedUsers();
            builder.Entity<EventuresUser>()
                .HasData(this.GuestUser);

            builder
                .Entity<Event>()
                .HasData(new Event() 
                { 
                    Id = 1,
                    Name = "Softuniada 2022",
                    Place = "Sofia",
                    Start = DateTime.ParseExact(DateTime.Now.AddDays(200)
                        .ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    End = DateTime.ParseExact(DateTime.Now.AddDays(201)
                        .ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    TotalTickets = 200,
                    PricePerTicket = 12.50M,
                    OwnerId = this.GuestUser.Id
                },
                new Event()
                {
                    Id = 2,
                    Name = "OpenFest 2022",
                    Place = "Online",
                    Start = DateTime.ParseExact(DateTime.Now.AddDays(500)
                        .ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    End = DateTime.ParseExact(DateTime.Now.AddDays(500).AddHours(8)
                        .ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    TotalTickets = 500,
                    PricePerTicket = 10.00M,
                    OwnerId = this.GuestUser.Id
                },
                new Event()
                {
                    Id = 3,
                    Name = "Microsoft Build 2022",
                    Place = "Online",
                    Start = DateTime.ParseExact(DateTime.Now.AddDays(300)
                        .ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    End = DateTime.ParseExact(DateTime.Now.AddDays(300).AddHours(12)
                    .ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                    TotalTickets = 1000,
                    PricePerTicket = 0.00m,
                    OwnerId = this.GuestUser.Id
                });

            base.OnModelCreating(builder);
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<EventuresUser>();

            this.GuestUser = new EventuresUser()
            {
                UserName = "guest",
                NormalizedUserName = "guest",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
                FirstName = "Guest",
                LastName = "User",
            };

            this.GuestUser.PasswordHash = hasher.HashPassword(this.GuestUser, "guest");
        }
    }
}
