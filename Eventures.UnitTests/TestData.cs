using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Eventures.App.Data;

namespace Eventures.UnitTests
{
    public class TestData
    {
        public ApplicationDbContext DbContext { get; private set; }
        public EventuresUser UserMaria { get; private set; }
        public Event EventSoftuniada { get; private set; }
        public Event EventOpenFest { get; private set; }

        public TestData()
        {
            // Create an in-memory database for testing
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("Eventures-MemoryDb-" + DateTime.Now.Ticks);
            DbContext = new ApplicationDbContext(optionsBuilder.Options);

            UserMaria = new EventuresUser()
            {
                Id = "25ab6879-32b1-4b44-b0f1-49e85a6418c9",
                UserName = "maria",
                Email = "maria@gmail.com",
                FirstName = "Maria",
                LastName = "Green",
            };
            DbContext.Add(UserMaria);
            var eventSoftuniada = new Event()
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
            DbContext.Add(eventSoftuniada);
            var eventOpenFest = new Event()
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
            DbContext.Add(eventOpenFest);
            DbContext.SaveChanges();
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
