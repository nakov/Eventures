using System;
using System.Linq;
using System.Globalization;
using System.Security.Claims;
using System.Collections.Generic;

using Eventures.Data;
using Eventures.WebApp.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Eventures.WebApp.Controllers
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
            List<EventViewModel> events = this.dbContext
                .Events
                .Include(e => e.Owner)
                .Select(e => CreateEventViewModel(e))
                .ToList();

            return View(events);
        }

        public IActionResult Create()
        {
            var currentDate = DateTime.Now;
            var startDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.AddHours(1).Hour, currentDate.Minute, 0);

            var nextDay = DateTime.Now.AddDays(1);
            var endDate = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, currentDate.AddHours(6).Hour, currentDate.Minute, 0);
           
            EventBindingModel model = new EventBindingModel()
            {
                Name = "New Event",
                Place = "Some Place",
                Start = startDate,
                End = endDate,
                PricePerTicket = 10.00M,
                TotalTickets = 100
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EventBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View(bindingModel);   
            }

            string currentUserId = GetUserId();
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
            this.dbContext.Events.Add(eventForDb);
            this.dbContext.SaveChanges();

            return RedirectToAction("All");
        }

        public IActionResult Delete(int id)
        {
            Event ev = this.dbContext.Events.Find(id);
            if (ev == null)
            {
                // When event with this id doesn't exist -> return "Bad Request"
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != ev.OwnerId)
            {
                // When current user is not an owner
                return Unauthorized();
            }

            return View(CreateEventViewModel(ev));
        }

        [HttpPost]
        public IActionResult Delete(EventViewModel eventModel)
        {
            Event ev = this.dbContext.Events.Find(eventModel.Id);
            if (ev == null)
            {
                // When event with this id doesn't exist
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != ev.OwnerId)
            {
                // Not an owner -> return "Unauthorized"
                return Unauthorized();
            }

            this.dbContext.Events.Remove(ev);
            this.dbContext.SaveChanges();

            return RedirectToAction("All");
        }

        public IActionResult Edit(int id)
        {
            Event ev = this.dbContext.Events.Find(id);
            if (ev == null)
            {
                // When event with this id doesn't exist
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != ev.OwnerId)
            {
                // When current user is not an owner
                return Unauthorized();
            }

            EventBindingModel model = new EventBindingModel()
            {
                Name = ev.Name,
                Place = ev.Place,
                Start = ev.Start,
                End = ev.End,
                PricePerTicket = ev.PricePerTicket,
                TotalTickets = ev.TotalTickets
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, EventBindingModel bindingModel)
        {
            Event ev = this.dbContext.Events.Find(id);
            if (ev == null)
            {
                // When event with this id doesn't exist
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != ev.OwnerId)
            {
                // Not an owner -> return "Unauthorized"
                return Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return View(bindingModel); 
            }

            ev.Name = bindingModel.Name;
            ev.Place = bindingModel.Place;
            ev.Start = bindingModel.Start;
            ev.End = bindingModel.End;
            ev.TotalTickets = bindingModel.TotalTickets;
            ev.PricePerTicket = bindingModel.PricePerTicket;

            this.dbContext.SaveChanges();
            return RedirectToAction("All");
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
                TotalTickets = ev.TotalTickets,
                PricePerTicket = ev.PricePerTicket,
                Owner = ev.Owner?.UserName
            };
        }

        private string GetUserId()
            => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
