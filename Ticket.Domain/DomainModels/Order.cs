using System;
using System.Collections.Generic;
using System.Text;

using Ticket.Domain.Relations;
using Ticket.Domain.Identity;

namespace Ticket.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public TicketApplicationUser User { get; set; }
        public double TotalPrice { get; set; }
        public DateTime TimeOrdered { get; set; }
        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }
    }
}
