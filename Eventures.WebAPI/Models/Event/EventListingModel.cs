using System;
using System.ComponentModel.DataAnnotations.Schema;

using Eventures.WebAPI.Models.User;

namespace Eventures.WebAPI.Models.Event
{
    public class EventListingModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Place { get; init; }
        public string Start { get; init; }
        public string End { get; init; }
        public int TotalTickets { get; init; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerTicket { get; init; }
        public UserListingModel Owner { get; init; }
    }
}
