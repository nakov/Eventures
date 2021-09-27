using System;
using System.ComponentModel.DataAnnotations;

using Eventures.Data;
using Eventures.Data.Attributes;

namespace Eventures.WebApp.Models
{
    using static DataConstants;
    public class EventBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(MaxEventName, MinimumLength = MinEventName,
            ErrorMessage = "{0} should be at least {2} characters long.")]
        public string Name { get; init; }

        [Required]
        [Display(Name = "Place")]
        [StringLength(MaxEventPlace, MinimumLength = MinEventPlace,
            ErrorMessage = "{0} should be at least {2} characters long.")]
        public string Place { get; init; }

        [Required]
        [Display(Name = "Start")]
        [CustomDateValidation]
        public DateTime Start { get; init; }

        [Required]
        [Display(Name = "End")]
        [DateGreaterThan("Start")]
        public DateTime End { get; init; }

        [Required]
        [Display(Name = "TotalTickets")]
        [Range(0, MaxEventTickets, 
            ErrorMessage = "Total Tickets must be a positive number and less than {2}.")]
        public int TotalTickets { get; init; }

        [Required]
        [Display(Name = "PricePerTicket")]
        [Range(0.00, MaxEventPrice, 
            ErrorMessage = "Price Per Ticket must be a positive number and less than {2}.")]
        public decimal PricePerTicket { get; init; }
    }
}
