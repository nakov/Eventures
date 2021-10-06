using System;
using System.Linq;
using System.Security.Claims;

using Eventures.Data;
using Eventures.WebAPI.Models;
using Eventures.WebAPI.Models.User;
using Eventures.WebAPI.Models.Event;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Eventures.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/events")]
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
        [AllowAnonymous]
        [HttpGet("count")]
        public IActionResult GetEventsCount()
        {
            return Ok(this.dbContext.Events.ToList().Count());
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
        [HttpGet]
        public IActionResult GetEvents()
        {
            var events = this.dbContext
                .Events
                .Select(e => new EventListingModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Place = e.Place,
                    Start = e.Start.ToString("dd/MM/yyyy HH:mm"),
                    End = e.End.ToString("dd/MM/yyyy HH:mm"),
                    TotalTickets = e.TotalTickets,
                    PricePerTicket = e.PricePerTicket,
                    Owner = new UserListingModel()
                    {
                        Id = e.OwnerId,
                        Username = e.Owner.UserName,
                        FirstName = e.Owner.FirstName,
                        LastName = e.Owner.LastName,
                        Email = e.Owner.Email
                    }
                })
                .ToList();

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
        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            var eventExists = this.dbContext.Events.Any(e => e.Id == id);
            if (!eventExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Event #{id} not found." });
            }

            var eventModel = CreateEventListingModelById(id);
            return Ok(eventModel);
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
        ///             "start": "2022-04-23T18:25:43.511Z",
        ///             "end": "2022-04-23T18:25:43.511Z",
        ///             "totalTickets": "155",
        ///             "pricePerTicket": "30.00"
        ///     }
        /// </remarks>
        /// <response code="201">Returns "Created" with the created event</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated</response>   
        [HttpPost("create")]
        public IActionResult CreateEvent(EventBindingModel bindingModel)
        {
            Event ev = new Event
            {
                Name = bindingModel.Name,
                Place = bindingModel.Place,
                Start = bindingModel.Start,
                End = bindingModel.End,
                TotalTickets = bindingModel.TotalTickets,
                PricePerTicket = bindingModel.PricePerTicket,
                OwnerId = GetCurrentUserId()
            };

            dbContext.Events.Add(ev);
            dbContext.SaveChanges();

            var eventModel = CreateEventListingModelById(ev.Id);

            return CreatedAtAction("GetEventById", new { id = ev.Id }, eventModel);
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
        ///             "start": "2022-04-23T18:25:43.511Z",
        ///             "end": "2022-04-23T18:25:43.511Z",
        ///             "totalTickets": "155",
        ///             "pricePerTicket": "30.00"
        ///     }
        /// </remarks>
        /// <response code="204">Returns "No Content"</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the event</response>  
        /// <response code="404">Returns "Not Found" when event with the given id doesn't exist</response>  
        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, EventBindingModel eventModel)
        {
            var eventExists = this.dbContext.Events.Any(e => e.Id == id);
            if (!eventExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Event #{id} not found." });
            }
            
            var ev = this.dbContext.Events.Find(id);

            if (GetCurrentUserId() != ev.OwnerId)
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot edit event, when not an owner." });
            }

            ev.Name = eventModel.Name;
            ev.Place = eventModel.Place;
            ev.Start = eventModel.Start.AddTicks(-(eventModel.Start.Ticks % TimeSpan.TicksPerSecond));
            ev.End = eventModel.End.AddTicks(-(eventModel.End.Ticks % TimeSpan.TicksPerSecond));
            ev.TotalTickets = eventModel.TotalTickets;
            ev.PricePerTicket = eventModel.PricePerTicket;
            this.dbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Partially edits an event.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the edited event!
        /// 
        /// Sample request:
        ///
        ///     PATCH /api/events/{id}
        ///     {
        ///             "name": "Changed Name"
        ///     }
        /// </remarks>
        /// <response code="204">Returns "No Content"</response>
        /// <response code="400">Returns "Bad Request" when an invalid request is sent</response>   
        /// <response code="401">Returns "Unauthorized" when user is not authenticated or is not the owner of the event</response>  
        /// <response code="404">Returns "Not Found" when event with the given id doesn't exist</response>  
        [HttpPatch("{id}")]
        public IActionResult PatchEvent(int id, PatchEventModel eventModel)
        {
            var eventExists = this.dbContext.Events.Any(e => e.Id == id);
            if (!eventExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Event #{id} not found." });
            }

            var ev = this.dbContext.Events.Find(id);

            if (GetCurrentUserId() != ev.OwnerId)
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot edit event, when not an owner." });
            }

            ev.Name = String.IsNullOrEmpty(eventModel.Name) ? ev.Name : eventModel.Name;
            ev.Place = String.IsNullOrEmpty(eventModel.Place) ? ev.Place : eventModel.Place;
            ev.Start = eventModel.Start == null ? ev.Start : eventModel.Start.Value;
            ev.End = eventModel.End == null ? ev.End : eventModel.End.Value;

            if(ev.Start > ev.End)
            {
                return BadRequest(
                    new ResponseMsg { Message = "End date must be after the start date." });
            }

            ev.Start = ev.Start.AddTicks(-(ev.Start.Ticks % TimeSpan.TicksPerSecond));
            ev.End = ev.End.AddTicks(-(ev.End.Ticks % TimeSpan.TicksPerSecond));

            ev.TotalTickets = eventModel.TotalTickets == null ? ev.TotalTickets : eventModel.TotalTickets.Value;
            ev.PricePerTicket = eventModel.PricePerTicket == null ? ev.PricePerTicket : eventModel.PricePerTicket.Value;
            this.dbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <remarks>
        /// You should be an authenticated user!
        /// You should be the owner of the deleted event!
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
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var eventExists = this.dbContext.Events.Any(e => e.Id == id);
            if (!eventExists)
            {
                return NotFound(
                    new ResponseMsg { Message = $"Event #{id} not found." });
            }

            var ev = this.dbContext.Events.Find(id);

            if (GetCurrentUserId() != ev.OwnerId)
            {
                return Unauthorized(
                    new ResponseMsg { Message = "Cannot delete event, when not an owner." });
            }

            var eventModel = CreateEventListingModelById(ev.Id);

            this.dbContext.Events.Remove(ev);
            this.dbContext.SaveChanges();

            return Ok(eventModel);
        }

        private EventListingModel CreateEventListingModelById(int id)
            => this.dbContext
                .Events
                .Where(e => e.Id == id)
                .Select(ev => new EventListingModel()
                {
                    Id = ev.Id,
                    Name = ev.Name,
                    Place = ev.Place,
                    Start = ev.Start.ToString("dd/MM/yyyy HH:mm"),
                    End = ev.End.ToString("dd/MM/yyyy HH:mm"),
                    TotalTickets = ev.TotalTickets,
                    PricePerTicket = ev.PricePerTicket,
                    Owner = new UserListingModel()
                    {
                        FirstName = ev.Owner.FirstName,
                        LastName = ev.Owner.LastName,
                        Id = ev.Owner.Id,
                        Username = ev.Owner.UserName,
                        Email = ev.Owner.Email
                    }
                })
                .FirstOrDefault();

        private string GetCurrentUserId()
        {
            string currentUsername = this.User.FindFirst(ClaimTypes.Name)?.Value;
            var currentUserId = this.dbContext
                .Users
                .FirstOrDefault(x => x.UserName == currentUsername)
                .Id;
            return currentUserId;
        }
    }
}
