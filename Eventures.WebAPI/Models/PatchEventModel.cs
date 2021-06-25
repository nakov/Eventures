using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.WebAPI.Models
{
    public class PatchEventModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Place")]
        public string Place { get; set; }

        [Display(Name = "Start")]
        public DateTime Start { get; set; }

        [Display(Name = "End")]
        public DateTime End { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Total Tickets must be a positive number.")]
        [Display(Name = "TotalTickets")]
        public int TotalTickets { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Price Per Ticket must be a positive number.")]
        [Display(Name = "PricePerTicket")]
        public decimal PricePerTicket { get; set; }
    }
}
