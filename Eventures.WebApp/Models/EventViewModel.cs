using System.ComponentModel.DataAnnotations;

namespace Eventures.WebApp.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Place { get; set; }

        [Display(Name = "Total Tickets")]
        public int TotalTickets { get; set; }

        [Display(Name = "Price Per Ticket")]
        public decimal PricePerTicket { get; set; }
        public string Owner { get; set; }
    }
}
