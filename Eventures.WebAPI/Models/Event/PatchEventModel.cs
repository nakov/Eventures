using System;
using System.ComponentModel.DataAnnotations;

using Eventures.Data;
using Eventures.Data.Attributes;

namespace Eventures.WebAPI.Models.Event
{
    using static DataConstants;
    public class PatchEventModel
    {
        [Display(Name = "Name")]
        [StringLength(MaxEventName, MinimumLength = MinEventName)]
        public string Name { get; init; }

        [Display(Name = "Place")]
        [StringLength(MaxEventPlace, MinimumLength = MinEventPlace)]
        public string Place { get; init; }

        [Display(Name = "Start")]
        [CustomDateValidation]
        public DateTime? Start { get; init; }

        [Display(Name = "End")]
        [DateGreaterThan("Start")]
        public DateTime? End { get; init; }

        [Display(Name = "TotalTickets")]
        [Range(0, MaxEventTickets)]
        public int? TotalTickets { get; init; }

        [Display(Name = "PricePerTicket")]
        [Range(0.00, MaxEventPrice)]
        public decimal? PricePerTicket { get; init; }
    }
}
