using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.Relations;
using System.ComponentModel.DataAnnotations;

namespace Ticket.Domain.DomainModels
{
    public class TicketProd : BaseEntity
    {
        [Required]
        public int TicketNumber { get; set; }
        [Required]
        public int TicketProjectionRoom { get; set; }
        [Required]
        public double TicketPrice { get; set; }
        [Required]
        [Range(1, 5)]
        public double TicketRating { get; set; }
        public virtual Movie Movie { get; set; }
        [Required]
        public DateTime dateValid { get; set; }
        public bool sold { get; set; }
        public virtual ICollection<TicketInMovie> TicketInMovie { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public virtual ICollection<TicketInOrder> TicketInOrders { get; set; }

    }
}
