using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventures.Data
{
    using static DataConstants;

    public class Event
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(MaxEventName)]
        public string Name { get; set; }

        [Required]
        [MaxLength(MaxEventPlace)]
        public string Place { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TotalTickets { get; set; }

        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerTicket { get; set; }

        [Required]
        public string OwnerId { get; init; }
        public EventuresUser Owner { get; init; }
    }
}
