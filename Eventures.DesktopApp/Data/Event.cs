using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventures.DesktopApp.Data
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Place { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int TotalTickets { get; set; }

        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerTicket { get; set; }

        public EventuresUser Owner { get; set; }
    }
}
