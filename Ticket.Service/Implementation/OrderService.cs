using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain;
using Ticket.Domain.DomainModels;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public List<Order> getAllOrders(string id)
        {
            return this._orderRepository.getAllOrders(id);
        }

        public Order getOrderDetails(Guid? id)
        {
            return this._orderRepository.getOrderDetails(id);
        }
    }
}
