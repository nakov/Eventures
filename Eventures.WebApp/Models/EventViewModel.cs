using System.ComponentModel.DataAnnotations;

namespace Eventures.WebApp.Models
{
    public class EventViewModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Start { get; init; }
        public string End { get; init; }
        public string Place { get; init; }

        [Display(Name = "Total Tickets")]
        public int TotalTickets { get; init; }

        [Display(Name = "Price Per Ticket")]
        public decimal PricePerTicket { get; init; }
        public string Owner { get; init; }
    }
}
