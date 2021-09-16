using System;
using System.ComponentModel.DataAnnotations;

namespace Eventures.WebAPI.Models.Event
{
    public class PatchEventModel
    {
        [Display(Name = "Name")]
        public string Name { get; init; }

        [Display(Name = "Place")]
        public string Place { get; init; }

        [Display(Name = "Start")]
        public DateTime? Start { get; init; }

        [Display(Name = "End")]
        public DateTime? End { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "Total Tickets must be a positive number.")]
        [Display(Name = "TotalTickets")]
        public int? TotalTickets { get; init; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Price Per Ticket must be a positive number.")]
        [Display(Name = "PricePerTicket")]
        public decimal? PricePerTicket { get; init; }
    }
}
