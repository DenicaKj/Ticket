using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain;
using Ticket.Domain.DomainModels;

namespace Ticket.Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> getAllOrders(string id);
        public Order getOrderDetails(Guid? id);

    }
}
