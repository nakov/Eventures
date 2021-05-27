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
        public Event EventSoftuniada { get; private set; }
        public Event EventOpenFest { get; private set; }
        private string uniqueDbName;

        public ApplicationDbContext CreateDbContext()
        {
            // Use in-memory database for testing
            // Attach the same DB every time, unless new TestDb() is called
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(uniqueDbName);
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            return dbContext;
        }

        public TestDb()
        {
            this.uniqueDbName = "Eventures-MemoryDb-" + DateTime.Now.Ticks;
            this.SeedDatabase();
        }

        public void SeedDatabase()
        {
            var dbContext = this.CreateDbContext();
            var userStore = new UserStore<EventuresUser>(dbContext);
            var userManager = new UserManager<EventuresUser>(userStore, null, new PasswordHasher<EventuresUser>(), null, null, null, null, null, null);

            this.UserMaria = new EventuresUser()
            {
                Id = "25ab6879-32b1-4b44-b0f1-49e85a6418c9",
                UserName = "maria",
                Email = "maria@gmail.com",
                FirstName = "Maria",
                LastName = "Green",
            };
            userManager.CreateAsync(this.UserMaria, this.UserMaria.UserName).Wait();

            this.EventSoftuniada = new Event()
            {
                Id = 1,
                Name = "Softuniada 2021",
                Place = "Sofia",
                Start = DateTime.Now.AddMonths(3),
                End = DateTime.Now.AddMonths(3),
                TotalTickets = 200,
                PricePerTicket = 12.00m,
                OwnerId = UserMaria.Id
            };
            dbContext.Add(this.EventSoftuniada);

            this.EventOpenFest = new Event()
            {
                Id = 2,
                Name = "OpenFest 2021",
                Place = "Online",
                Start = DateTime.Now.AddDays(200),
                End = DateTime.Now.AddDays(201),
                TotalTickets = 5000,
                PricePerTicket = 0,
                OwnerId = UserMaria.Id
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
