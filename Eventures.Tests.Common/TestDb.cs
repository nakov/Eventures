﻿using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Eventures.Data;

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

        /// <summary>
        /// Creates a new ApplicationDbContext, which connects to the existing database, 
        /// which was already created and initialized with the constructor `new TestDb()`.
        /// </summary>
        /// <returns></returns>
        public ApplicationDbContext CreateDbContext()
        {
            // Use in-memory database for testing
            // Attach the same DB every time, unless new TestDb() is called
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(uniqueDbName);
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=" + uniqueDbName);
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;
        }

        /// <summary>
        /// Creates a new testing database and resets its data to its initial state.
        /// </summary>
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
