using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventures.WebAPI.Models
{
    public class ApiEventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TotalTickets { get; set; }

        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerTicket { get; set; }

        public ApiUserViewModel Owner { get; set; }
    }
}
