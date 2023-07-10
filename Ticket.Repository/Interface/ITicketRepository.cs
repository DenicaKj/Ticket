using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;

namespace Ticket.Repository.Interface
{
    public interface ITicketRepository
    {
        IEnumerable<TicketProd> GetAll();
        IEnumerable<TicketProd> GetByDateValid(DateTime selectedDate);
        IEnumerable<TicketProd> GetAllNotSold();
        TicketProd Get(Guid? id);
        void Insert(TicketProd entity);
        void Update(TicketProd entity);
        void Delete(TicketProd entity);

    }
}
