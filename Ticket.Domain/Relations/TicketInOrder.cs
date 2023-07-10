using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;

namespace Ticket.Domain.Relations
{
    public class TicketInOrder:BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public TicketProd Ticket { get; set; }
        public int Quantity { get; set; }
    }

}
