﻿using System;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Eventures.App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Eventures.UnitTests
{
    public class TestDb
    {
        public EventuresUser UserMaria { get; private set; }
        public EventuresUser UserPeter { get; private set; }
        public Event EventSoftuniada { get; private set; }
        public Event EventOpenFest { get; private set; }
        private string uniqueDbName;

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

        public TestDb()
        {
            this.uniqueDbName = "Eventures-TestDb-" + DateTime.Now.Ticks;
            this.SeedDatabase();
        }

        public void SeedDatabase()
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

            dbContext.SaveChanges();
        }

        public static void AssignCurrentUserForController(
            Controller controller, EventuresUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName + " " + user.LastName),
            };
            var userIdentity = new ClaimsIdentity(userClaims, "Password");
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userPrincipal }
            };
        }
    }
}
