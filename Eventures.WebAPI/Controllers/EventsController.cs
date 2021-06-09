using Eventures.App.Data;
using Eventures.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Eventures.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
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
        public ActionResult<IEnumerable<Event>> GetEvents()
        {
            var events = this.dbContext.Events.ToList();
            foreach (var ev in events)
            {
                ev.Owner = this.dbContext.Users.Find(ev.OwnerId);
            }
            return this.dbContext.Events.ToList();
        }

        [Authorize]
        [HttpGet("{id}")] // GET: /api/events/1
        public ActionResult<Event> GetEventById(int id)
        {
            var ev = this.dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound();
            }
            ev.Owner = this.dbContext.Users.Find(ev.OwnerId);
            return Ok(ev);
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

            return CreatedAtAction("GetEventById", new { id = ev.Id }, ev);
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
        public ActionResult<Event> Delete(int id)
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
            return Ok(ev);
        }
    }
}
