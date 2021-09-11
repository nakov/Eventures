using System;
using System.ComponentModel.DataAnnotations;

namespace Eventures.WebAPI.Models.Event
{
    public class EventBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Place")]
        public string Place { get; set; }

        [Required]
        [Display(Name = "Start")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "End")]
        public DateTime End { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Total Tickets must be a positive number.")]
        [Display(Name = "TotalTickets")]
        public int TotalTickets { get; set; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Price Per Ticket must be a positive number.")]
        [Display(Name = "PricePerTicket")]
        public decimal PricePerTicket { get; set; }
    }
}
