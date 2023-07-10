using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain;
using Ticket.Domain.DomainModels;

namespace Ticket.Service.Interface
{
    public interface IOrderService
    {
        public List<Order> getAllOrders(string id);
        public Order getOrderDetails(Guid? id);
    }
}
