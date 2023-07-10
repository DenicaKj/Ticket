using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;

namespace Ticket.Domain.Relations
{
    public class TicketInMovie:BaseEntity
    {
        public Guid MovieId { get; set; }
        public Movie Movie{ get; set; }

        public Guid TicketId { get; set; }
        public TicketProd Ticket { get; set; }
    }
}
