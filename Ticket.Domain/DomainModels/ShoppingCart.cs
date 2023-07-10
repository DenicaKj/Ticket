using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.Relations;
using Ticket.Domain.Identity;

namespace Ticket.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual TicketApplicationUser Owner { get; set; }

        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }

    }
}
