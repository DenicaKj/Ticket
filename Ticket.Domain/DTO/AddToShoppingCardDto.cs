using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;

namespace Ticket.Domain.DTO
{
    public class AddToShoppingCardDto
    {
        public TicketProd SelectedTicket { get; set; }
        public Guid SelectedTicketId { get; set; }
        public int Quantity { get; set; }
    }
}
