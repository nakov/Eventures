using System;

using Eventures.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Eventures.Tests.Common
{
    public class TestDb
    {
        public EventuresUser UserMaria { get; private set; }
        public EventuresUser UserPeter { get; private set; }
        public Event EventSoftuniada { get; private set; }
        public Event EventOpenFest { get; private set; }
        public Event EventMSBuild { get; private set; }      
        private string uniqueDbName;

        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(uniqueDbName);
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;
        }

        public TestDb()
        {
            this.uniqueDbName = "Eventures-TestDb-" + DateTime.Now.Ticks;
            this.SeedDatabase();
        }

        private void SeedDatabase()
        {
            var dbContext = this.CreateDbContext();
            var userStore = new UserStore<EventuresUser>(dbContext);
            var hasher = new PasswordHasher<EventuresUser>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var userManager = new UserManager<EventuresUser>(
                userStore, null, hasher, null, null, normalizer, null, null, null);

            this.UserMaria = new EventuresUser()
            {
                UserName = "maria",
                Email = "maria@gmail.com",
                FirstName = "Maria",
                LastName = "Green",
            };
            userManager.CreateAsync(this.UserMaria, this.UserMaria.UserName).Wait();

            this.UserPeter = new EventuresUser()
            {
                UserName = "peter",
                Email = "peter@gmail.com",
                FirstName = "Peter",
                LastName = "Newton",
            };
            userManager.CreateAsync(this.UserPeter, this.UserPeter.UserName).Wait();

            // EventSoftuniada has owner UserMaria
            this.EventSoftuniada = new Event()
            {
                Name = "Softuniada 2021",
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 200,
                PricePerTicket = 12.00m,
                OwnerId = UserMaria.Id
            };
            dbContext.Add(this.EventSoftuniada);

            // EventOpenFest has owner UserPeter
            this.EventOpenFest = new Event()
            {
                Name = "OpenFest 2021",
                Place = "Online",
                Start = DateTime.Now.AddDays(200),
                End = DateTime.Now.AddDays(201),
                TotalTickets = 5000,
                PricePerTicket = 10.00m,
                OwnerId = UserPeter.Id
            };
            dbContext.Add(this.EventOpenFest);

            // EventOpenFest has owner UserPeter
            this.EventMSBuild = new Event()
            {
                Name = "Microsoft Build 2021",
                Place = "Online",
                Start = DateTime.Now.AddDays(300),
                End = DateTime.Now.AddDays(302),
                TotalTickets = 25000,
                PricePerTicket = 0.00m,
                OwnerId = UserPeter.Id
            };
            dbContext.Add(this.EventMSBuild);

            dbContext.SaveChanges();
        }
    }
}
