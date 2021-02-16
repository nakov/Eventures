using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Eventures.App.Data;
using Eventures.App.Domain;
using System;
using Eventures.App.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Eventures.App.Models;

namespace Eventures.UnitTests
{
    public class EventControllerTests
    {
        private ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("Eventures-MemoryDb");
            dbContext = new ApplicationDbContext(optionsBuilder.Options);
            var userMaria = new EventuresUser()
            {
                Id = "25ab6879-32b1-4b44-b0f1-49e85a6418c9",
                FirstName = "Maria",
                LastName = "Green",
                UserName = "maria",
                Email = "maria@gmail.com",
            };
            dbContext.Add(userMaria);
            var eventSoftuniada = new Event()
            {
                Id = 1,
                Name = "Softuniada 2021",
                Place = "Sofia",
                Start = DateTime.Now,
                End = DateTime.Now,
                TotalTickets = 200,
                PricePerTicket = 12.00m,
                OwnerId = userMaria.Id
            };
            dbContext.Add(eventSoftuniada);
            var eventOpenFest = new Event()
            {
                Id = 2,
                Name = "OpenFest 2021",
                Place = "Online",
                Start = DateTime.Now.AddDays(200),
                End = DateTime.Now.AddDays(200),
                TotalTickets = 5000,
                PricePerTicket = 0,
                OwnerId = userMaria.Id
            };
            dbContext.Add(eventOpenFest);
            dbContext.SaveChanges();
        }

        [Test]
        public void TestEventsControllerAll()
        {
            var controller = new EventsController(this.dbContext);

            // Act
            var result = controller.All();

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<EventAllViewModel>;

            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("Softuniada 2021", model[0].Name);
            Assert.AreEqual("OpenFest 2021", model[1].Name);
        }
    }
}
