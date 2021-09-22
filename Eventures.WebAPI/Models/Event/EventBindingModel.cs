using System;
using System.ComponentModel.DataAnnotations;

using Eventures.Data;
using Eventures.Data.Attributes;

namespace Eventures.WebAPI.Models.Event
{
    using static DataConstants;
    public class EventBindingModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(MaxEventName, MinimumLength = MinEventName)]
        public string Name { get; init; }

        [Required]
        [Display(Name = "Place")]
        [StringLength(MaxEventPlace, MinimumLength = MinEventPlace)]
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
        [Range(0, MaxEventTickets)]
        public int TotalTickets { get; init; }

        [Required]
        [Display(Name = "PricePerTicket")]
        [Range(0.00, MaxEventPrice)]
        public decimal PricePerTicket { get; init; }
    }
}
