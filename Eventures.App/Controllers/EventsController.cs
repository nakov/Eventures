using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Eventures.App.Data;
using Eventures.App.Models;

namespace Eventures.App.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EventsController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        public IActionResult All()
        {
            List<EventViewModel> events = dbContext.Events
                .Include(e => e.Owner)
                .Select(e => CreateEventViewModel(e))
                .ToList();
            return this.View(events);
        }

        public IActionResult Create()
        {
            EventCreateBindingModel model = new EventCreateBindingModel()
            {
                Name = "New Event",
                Place = "Some Place",
                Start = DateTime.Now.Date.AddDays(7).AddHours(10),
                End = DateTime.Now.Date.AddDays(7).AddHours(18),
                PricePerTicket = 10,
                TotalTickets = 100
            };
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(EventCreateBindingModel bindingModel)
        {
            if (this.ModelState.IsValid)
            {
                string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Event eventForDb = new Event
                {
                    Name = bindingModel.Name,
                    Place = bindingModel.Place,
                    Start = bindingModel.Start,
                    End = bindingModel.End,
                    TotalTickets = bindingModel.TotalTickets,
                    PricePerTicket = bindingModel.PricePerTicket,
                    OwnerId = currentUserId
                };
                dbContext.Events.Add(eventForDb);
                dbContext.SaveChanges();

                return this.RedirectToAction("All");
            }

            return this.View();
        }

        public IActionResult Delete(int id)
        {
            Event ev = dbContext.Events.Find(id);
            string currentUser = this.User.FindFirstValue(ClaimTypes.Name);
            if (ev == null || currentUser != this.dbContext.Users.Find(ev.OwnerId).UserName)
                return this.View();
            return this.View(CreateEventViewModel(ev));
        }

        [HttpPost]
        public IActionResult Delete(EventViewModel eventModel)
        {
            Event ev = dbContext.Events.Find(eventModel.Id);
            if (ev != null)
            {
                dbContext.Events.Remove(ev);
                dbContext.SaveChanges();
                return this.RedirectToAction("All");
            }
            return this.View();
        }

        private static EventViewModel CreateEventViewModel(Event ev)
        {
            return new EventViewModel
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Start = ev.Start.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture),
                End = ev.End.ToString("dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture),
                Owner = ev.Owner?.UserName
            };
        }
    }
}
