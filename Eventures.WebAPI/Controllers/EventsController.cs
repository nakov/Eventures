using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApi.Models;

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
        public ActionResult<IEnumerable<ApiEventViewModel>> GetEvents()
        {
            var events = new List<ApiEventViewModel>();

            var dbEvents = this.dbContext.Events.ToList();
            foreach (var ev in dbEvents)
            {
                var owner = this.dbContext.Users.Find(ev.OwnerId);
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
                        FirstName = owner.FirstName,
                        LastName = owner.LastName,
                        Id = owner.Id,
                        Username = owner.UserName,
                        Email = owner.Email
                    }
                };
                events.Add(eventModel);
            }
            return events;
        }

        [Authorize]
        [HttpGet("{id}")] // GET: /api/events/1
        public ActionResult<ApiEventViewModel> GetEventById(int id)
        {
            var dbEvent = this.dbContext.Events.Find(id);
            if (dbEvent == null)
            {
                return NotFound();
            }
            var owner = this.dbContext.Users.Find(dbEvent.OwnerId);
            var eventModel = new ApiEventViewModel()
            {
                Id = dbEvent.Id,
                Name = dbEvent.Name,
                Place = dbEvent.Place,
                Start = dbEvent.Start,
                End = dbEvent.End,
                TotalTickets = dbEvent.TotalTickets,
                PricePerTicket = dbEvent.PricePerTicket,
                Owner = new ApiUserViewModel()
                {
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    Id = owner.Id,
                    Username = owner.UserName,
                    Email = owner.Email
                }
            };
            return Ok(eventModel);
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
