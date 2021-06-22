using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Eventures.App.Data;
using Eventures.App.Models;
using Eventures.WebAPI.Models;

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

        /// <summary>
        /// Gets events count.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/events/count
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with events count</response>  
        [HttpGet("count")] // GET: /api/events/count
        public int GetEventsCount()
        {
            return this.dbContext.Events.ToList().Count();
        }

        /// <summary>
        /// Gets a list with all events.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     GET /api/events
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with a list of all events</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>    
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

        /// <summary>
        /// Gets an event by id.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     GET /api/events/{id}
        ///     {
        ///         
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with the event</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>    
        /// <response code="404">Returns "Not Found" when event with the given id doesn't exist</response>    
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

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// 
        /// Sample request:
        ///
        ///     POST /api/events/create
        ///     {
        ///             "name": "Party",
        ///             "place": "Online",
        ///             "start": "2012-04-23T18:25:43.511Z",
        ///             "end": "2012-04-23T18:25:43.511Z",
        ///             "totalTickets": "155",
        ///             "pricePerTicket": "30.00"
        ///     }
        /// </remarks>
        /// <response code="201">Returns "Created" with the created event</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>   
        [Authorize]
        [HttpPost("create")]
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

        /// <summary>
        /// Edits an event.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the edited event!
        /// 
        /// Sample request:
        ///
        ///     PUT /api/events/{id}
        ///     {
        ///             "name": "Changed Name",
        ///             "place": "Online",
        ///             "start": "2012-04-23T18:25:43.511Z",
        ///             "end": "2012-04-23T18:25:43.511Z",
        ///             "totalTickets": "155",
        ///             "pricePerTicket": "30.00"
        ///     }
        /// </remarks>
        /// <response code="204">Returns "No Content"</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the event</response>  
        /// <response code="404">Returns "Not Found" when event with the given id doesn't exist</response>  
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, EventCreateBindingModel eventModel)
        {
            var ev = dbContext.Events.Find(id);
            if (ev == null)
            {
                return NotFound(new { msg = $"Event #{id} not found." });
            }

            string currentUsername = this.User.FindFirst(ClaimTypes.Name)?.Value;
            var currentUser = this.dbContext.Users.FirstOrDefault(x => x.UserName == currentUsername);
            if(currentUser.Id != ev.OwnerId)
            {
                return Unauthorized(new { msg = "Cannot edit event, when not an owner." });
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

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the edited event!
        /// 
        /// Sample request:
        ///
        ///     DELETE /api/events/{id}
        ///     {
        ///            
        ///     }
        /// </remarks>
        /// <response code="200">Returns "OK" with the deleted event</response>
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the event</response>  
        /// <response code="404">Returns "Not Found" when event with the given id doesn't exist</response>  
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
