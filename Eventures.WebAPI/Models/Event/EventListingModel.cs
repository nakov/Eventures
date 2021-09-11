using System;
using System.ComponentModel.DataAnnotations.Schema;

using Eventures.WebAPI.Models.User;

namespace Eventures.WebAPI.Models.Event
{
    public class EventListingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int TotalTickets { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerTicket { get; set; }
        public UserListingModel Owner { get; set; }
    }
}
