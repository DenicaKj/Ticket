using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.Relations;

namespace Ticket.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<TicketInShoppingCart> Products { get; set; }

        public double TotalPrice { get; set; }
    }
}
