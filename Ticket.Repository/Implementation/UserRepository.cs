using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticket.Domain.Identity;
using Ticket.Repository.Interface;
using Ticket.Web.Data;

namespace Ticket.Repository.Implementation
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<TicketApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TicketApplicationUser>();
        }
        public IEnumerable<TicketApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public TicketApplicationUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.TicketInShoppingCarts")
               .Include("UserCart.TicketInShoppingCarts.currentTicket")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(TicketApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(TicketApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TicketApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
