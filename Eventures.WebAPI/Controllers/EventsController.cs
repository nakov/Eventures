using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using WebApi.Models;

namespace Eventures.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EventsController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet("count")] // GET: /api/events/count
        public int GetEventsCount()
        {
            return this.dbContext.Events.ToList().Count();
        }

        [Authorize]
        [HttpGet] // GET: /api/events
        public IActionResult GetEvents()
        {
            var events = new List<ApiEventViewModel>();

            var dbEvents = this.dbContext.Events.ToList();
            foreach (var ev in dbEvents)
            {
                ev.Owner = this.dbContext.Users.Find(ev.OwnerId);
                var eventViewModel = CreateViewModel(ev);
                events.Add(eventViewModel);
            }
            return Ok(events);
        }

        [Authorize]
        [HttpGet("{id}")] // GET: /api/events/1
        public IActionResult GetEventById(int id)
        {
            var dbEvent = this.dbContext.Events.Find(id);
            if (dbEvent == null)
            {
                return NotFound();
            }
            dbEvent.Owner = this.dbContext.Users.Find(dbEvent.OwnerId);
            var eventViewModel = CreateViewModel(dbEvent);
            return Ok(eventViewModel);
        }

        [Authorize]
        [HttpPost("create")] // POST: api/events/create
        public IActionResult CreateEvent(EventCreateBindingModel bindingModel)
        {
            string currentUsername = this.User.FindFirst(ClaimTypes.Name)?.Value;
            var currentUser = this.dbContext.Users.FirstOrDefault(x => x.UserName == currentUsername);
            Event ev = new Event
            {
                Name = bindingModel.Name,
                Place = bindingModel.Place,
                Start = bindingModel.Start,
                End = bindingModel.End,
                TotalTickets = bindingModel.TotalTickets,
                PricePerTicket = bindingModel.PricePerTicket,
                Owner = currentUser,
                OwnerId = currentUser.Id
            };
            dbContext.Events.Add(ev);
            dbContext.SaveChanges();

            var eventViewModel = CreateViewModel(ev);

            return CreatedAtAction("GetEventById", new { id = ev.Id }, eventViewModel);
        }

        [Authorize]
        [HttpPut("{id}")] // PUT: api/events/1
        public IActionResult PutEvent(int id, EventCreateBindingModel eventModel)
        {
            var ev = dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound();
            }

            string currentUsername = this.User.FindFirst(ClaimTypes.Name)?.Value;
            var currentUser = this.dbContext.Users.FirstOrDefault(x => x.UserName == currentUsername);
            if(currentUser.Id != ev.OwnerId)
            {
                return Unauthorized();
            }

            ev.Name = eventModel.Name;
            ev.Place = eventModel.Place;
            ev.Start = eventModel.Start;
            ev.End = eventModel.End;
            ev.TotalTickets = eventModel.TotalTickets;
            ev.PricePerTicket = eventModel.PricePerTicket;
            dbContext.SaveChanges();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")] // DELETE: api/events/1
        public IActionResult DeleteEvent(int id)
        {
            Event ev = dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound();
            };

            string currentUsername = this.User.FindFirst(ClaimTypes.Name)?.Value;
            var currentUser = this.dbContext.Users.FirstOrDefault(x => x.UserName == currentUsername);
            if (currentUser.Id != ev.OwnerId)
            {
                return Unauthorized();
            }
            dbContext.Events.Remove(ev);
            dbContext.SaveChanges();

            var eventViewModel = CreateViewModel(ev);
            
            return Ok(eventViewModel);
        }

        private ApiEventViewModel CreateViewModel(Event ev)
        {
            var eventModel = new ApiEventViewModel()
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Start = ev.Start,
                End = ev.End,
                TotalTickets = ev.TotalTickets,
                PricePerTicket = ev.PricePerTicket,
                Owner = new ApiUserViewModel()
                {
                    FirstName = ev.Owner.FirstName,
                    LastName = ev.Owner.LastName,
                    Id = ev.Owner.Id,
                    Username = ev.Owner.UserName,
                    Email = ev.Owner.Email
                }
            };

            return eventModel;
        }
    }
}
