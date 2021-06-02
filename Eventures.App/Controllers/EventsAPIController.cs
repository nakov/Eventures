using Eventures.App.Data;
using Eventures.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Eventures.App.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EventsAPIController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        // Allow anonymous
        [HttpGet] // GET: /api/eventsapi
        public ActionResult<IEnumerable<Event>> GetEventsCount() 
        {
            return this.dbContext.Events.ToList();
        }

        // Allow authenticated users only
        [HttpGet] // GET: /api/eventsapi
        public ActionResult<IEnumerable<Event>> GetEvents()
        {
            return this.dbContext.Events.ToList();
        }

        // Allow authenticated users only
        [HttpGet("{id}")] // GET: /api/eventsapi/1
        public ActionResult<Event> GetEventById(int id)
        {
            var ev = this.dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound();
            }
            return Ok(ev);
        }

        // Allow authenticated users only
        [HttpPost] // POST: api/eventsapi
        public IActionResult Create(EventCreateBindingModel bindingModel)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Event ev = new Event
            {
                Name = bindingModel.Name,
                Place = bindingModel.Place,
                Start = bindingModel.Start,
                End = bindingModel.End,
                TotalTickets = bindingModel.TotalTickets,
                PricePerTicket = bindingModel.PricePerTicket,
                OwnerId = currentUserId
            };
            dbContext.Events.Add(ev);
            dbContext.SaveChanges();

            return CreatedAtAction("GetEventById", new { id = ev.Id }, ev);
        }

        // Allow authenticated event owner only
        [HttpPut("{id}")] // PUT: api/eventsapi/1
        public IActionResult PutProduct(int id, EventCreateBindingModel eventModel)
        {
            var ev = dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound();
            }

            // TODO: check event owner

            ev.Name = eventModel.Name;
            ev.Place = eventModel.Place;
            ev.Start = eventModel.Start;
            ev.End = eventModel.End;
            ev.TotalTickets = eventModel.TotalTickets;
            ev.PricePerTicket = eventModel.PricePerTicket;
            dbContext.SaveChanges();
            return NoContent();
        }

        // Allow authenticated event owner only
        [HttpDelete("{id}")] // DELETE: api/eventsapi/1
        public ActionResult<Event> Delete(int id)
        {
            Event ev = dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound();
            };

            // TODO: check event owner

            dbContext.Events.Remove(ev);
            dbContext.SaveChanges();
            return ev;
        }
    }
}
