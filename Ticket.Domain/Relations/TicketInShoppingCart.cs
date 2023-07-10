using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;

namespace Ticket.Domain.Relations
{
    public class TicketInShoppingCart : BaseEntity
    {
        public Guid ProductId { get; set; }
        public virtual TicketProd currentTicket { get; set; }

        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart UserCart { get; set; }
        public int Quantity { get; set; }
    }
}
