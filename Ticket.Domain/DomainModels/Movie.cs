using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ticket.Domain.Relations;

namespace Ticket.Domain.DomainModels
{
    public class Movie:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        [Range(1, 5)]
        public double MovieRating { get; set; }
        public ICollection<TicketInMovie> TicketInMovie { get; set; }
    }
}
