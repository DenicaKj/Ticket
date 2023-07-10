using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticket.Domain.DomainModels;
using Ticket.Repository.Interface;
using Ticket.Web.Data;

namespace Ticket.Repository.Implementation
{
   
        public class TicketRepository : ITicketRepository
        {
            private readonly ApplicationDbContext context;
            private DbSet<TicketProd> entities;
            string errorMessage = string.Empty;

            public TicketRepository(ApplicationDbContext context)
            {
                this.context = context;
                entities = context.Set<TicketProd>();
            }
            public IEnumerable<TicketProd> GetAll()
            {
                return entities.Include(z=>z.TicketInMovie).ThenInclude(tim=>tim.Movie).AsEnumerable();
            }

            public TicketProd Get(Guid? id)
            {
                return entities.SingleOrDefault(s => s.Id == id);
            }
            public void Insert(TicketProd entity)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.Add(entity);
                context.SaveChanges();
            }

            public void Update(TicketProd entity)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.Update(entity);
                context.SaveChanges();
            }

            public void Delete(TicketProd entity)
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.Remove(entity);
                context.SaveChanges();
            }
        public IEnumerable<TicketProd> GetByDateValid(DateTime selectedDate)
        {
            return entities.Include(t => t.TicketInMovie).ThenInclude(tim=>tim.Movie).Where(t => t.dateValid.Date == selectedDate.Date).AsEnumerable();
        }

        public IEnumerable<TicketProd> GetAllNotSold()
        {
           
            return entities.Include(t => t.TicketInMovie).ThenInclude(tim => tim.Movie)
                    .Where(t => !t.sold)
                    .AsEnumerable();
        }
    }
    
}
