using System;

using Eventures.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Eventures.Tests.Common
{
    public class TestDb
    {
        private string uniqueDbName;

        public TestDb()
        {
            this.uniqueDbName = "Eventures-TestDb-" + DateTime.Now.Ticks;
            this.SeedDatabase();
        }

        public EventuresUser GuestUser { get; private set; }
        public EventuresUser UserMaria { get; private set; }
        public Event EventOpenFest { get; private set; }
        public Event EventDevConf { get; private set; }


        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(uniqueDbName);
            return new ApplicationDbContext(optionsBuilder.Options, false);
        }

        private void SeedDatabase()
        {
            var dbContext = this.CreateDbContext();
            var userStore = new UserStore<EventuresUser>(dbContext);
            var hasher = new PasswordHasher<EventuresUser>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var userManager = new UserManager<EventuresUser>(
                userStore, null, hasher, null, null, normalizer, null, null, null);

            // Create GuestUser
            this.GuestUser = new EventuresUser()
            {
                UserName = "guest",
                NormalizedUserName = "guest",
                Email = "guest@mail.com",
                NormalizedEmail = "guest@mail.com",
                FirstName = "Guest",
                LastName = "User",
            };
            userManager.CreateAsync(this.GuestUser, this.GuestUser.UserName).Wait();

            // EventOpenFest has owner GuestUser
            this.EventOpenFest = new Event()
            {
                Name = "OpenFest",
                Place = "Online",
                Start = DateTime.Now.AddDays(500),
                End = DateTime.Now.AddDays(500).AddHours(8),
                TotalTickets = 500,
                PricePerTicket = 10.00M,
                OwnerId = this.GuestUser.Id
            };

            dbContext.Add(this.EventOpenFest);

            // Create UserMaria
            this.UserMaria = new EventuresUser()
            {
                UserName = "maria",
                Email = "maria@gmail.com",
                FirstName = "Maria",
                LastName = "Green",
            };
            userManager.CreateAsync(this.UserMaria, this.UserMaria.UserName).Wait();

            // EventDevConf has owner UserMaria
            this.EventDevConf = new Event()
            {
                Name = "Dev Conference",
                Place = "Varna",
                Start = DateTime.Now.AddMonths(5),
                End = DateTime.Now.AddMonths(5).AddDays(5),
                TotalTickets = 350,
                PricePerTicket = 20.00m,
                OwnerId = this.UserMaria.Id
            };
            dbContext.Add(this.EventDevConf);

            dbContext.SaveChanges();
        }
    }
}
