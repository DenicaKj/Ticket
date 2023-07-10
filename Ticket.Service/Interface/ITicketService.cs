using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;
using Ticket.Domain.DTO;

namespace Ticket.Service.Interface
{
    public interface ITicketService
    {
        List<TicketProd> GetAllProducts();
        List<TicketProd> GetAllNotSoldProducts();
        List<TicketProd> GetByDateValid(DateTime selectedDate);
        TicketProd GetDetailsForProduct(Guid? id);
        void CreateNewProduct(TicketProd p,Guid id);
        void UpdeteExistingProduct(TicketProd p);
        AddToShoppingCardDto GetShoppingCartInfo(Guid? id);
        void DeleteProduct(Guid id);
        bool AddToShoppingCart(AddToShoppingCardDto item, string userID);
    }
}
