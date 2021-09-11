using System;
using System.ComponentModel.DataAnnotations;

namespace Eventures.WebAPI.Models.Event
{
    public class EventBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; init; }

        [Required]
        [Display(Name = "Place")]
        public string Place { get; init; }

        [Required]
        [Display(Name = "Start")]
        public DateTime Start { get; init; }

        [Required]
        [Display(Name = "End")]
        public DateTime End { get; init; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Total Tickets must be a positive number.")]
        [Display(Name = "TotalTickets")]
        public int TotalTickets { get; init; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Price Per Ticket must be a positive number.")]
        [Display(Name = "PricePerTicket")]
        public decimal PricePerTicket { get; init; }
    }
}
