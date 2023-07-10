using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DTO;

namespace Ticket.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteProductFromSoppingCart(string userId, Guid productId);
        bool order(string userId);
    }
}
