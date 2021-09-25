using System;
using System.Threading.Tasks;

using Eventures.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Eventures.Tests.Common
{
    public class TestDb
    {
        private ApplicationDbContext dbContext;
        private string uniqueDbName;

        public TestDb()
        {
            this.uniqueDbName = "Eventures-TestDb-" + DateTime.Now.Ticks;
            this.SeedDatabase().Wait();
        }

        public EventuresUser GuestUser { get; private set; }
        public EventuresUser UserMaria { get; private set; }
        public Event EventDevConf { get; private set; }
        public Event EventSoftuniada { get; private set; }
        public Event EventOpenFest { get; private set; }
        public Event EventMSBuild { get; private set; }


        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(uniqueDbName);
            dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;
        }

        private async Task SeedDatabase()
        {
            var dbContext = this.CreateDbContext();
            var userStore = new UserStore<EventuresUser>(dbContext);
            var hasher = new PasswordHasher<EventuresUser>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var userManager = new UserManager<EventuresUser>(
                userStore, null, hasher, null, null, normalizer, null, null, null);

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

            // Get GuestUser for the database
            this.GuestUser = await this.dbContext.Users
                .FirstOrDefaultAsync(u => u.UserName == "guest");

            // Get events for the database
            this.EventSoftuniada = await this.dbContext.Events
                .FirstOrDefaultAsync(e => e.Name.Contains("Softuniada"));
            this.EventOpenFest = await this.dbContext.Events
                .FirstOrDefaultAsync(e => e.Name.Contains("OpenFest")); ;
            this.EventMSBuild = await this.dbContext.Events
                .FirstOrDefaultAsync(e => e.Name.Contains("Microsoft"));
        }
    }
}
