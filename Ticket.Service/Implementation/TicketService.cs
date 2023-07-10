using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticket.Domain.DomainModels;
using Ticket.Domain.DTO;
using Ticket.Domain.Relations;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IRepository<TicketInMovie> _ticketInMovieRepository;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IUserRepository _userRepository;
        public TicketService(IRepository<Movie> movieRepository,IRepository<TicketInMovie> ticketInMovieRepository,ITicketRepository ticketRepository, IUserRepository userRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository)
        {
            _movieRepository = movieRepository;
            _ticketInMovieRepository = ticketInMovieRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
        }
        public bool AddToShoppingCart(AddToShoppingCardDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item.SelectedTicketId != null && userShoppingCard != null)
            {
                var product = this.GetDetailsForProduct(item.SelectedTicketId);

                if (product != null)
                {
                    TicketInShoppingCart itemToAdd = new TicketInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        currentTicket = product,
                        ProductId = product.Id,
                        UserCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };

                    var existing = userShoppingCard.TicketInShoppingCarts.Where(z => z.ShoppingCartId == userShoppingCard.Id && z.ProductId == itemToAdd.ProductId).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.Quantity += itemToAdd.Quantity;
                        this._ticketInShoppingCartRepository.Update(existing);

                    }
                    else
                    {
                        this._ticketInShoppingCartRepository.Insert(itemToAdd);
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewProduct(TicketProd p,Guid id)
        {
            if (p != null)
            {
                var movie = _movieRepository.Get(id);
                TicketInMovie itemToAdd = new TicketInMovie
                {
                    Id = Guid.NewGuid(),
                    MovieId =id,
                    Movie=movie,
                    Ticket=p,
                    TicketId=p.Id

                };
                p.Movie = movie;
                this._ticketRepository.Insert(p);
                var existing = p.TicketInMovie?.FirstOrDefault(z => z.MovieId == id);

                if (existing != null)
                {
                    this._ticketInMovieRepository.Update(existing);

                }
                else
                {
                    this._ticketInMovieRepository.Insert(itemToAdd);
                }
                
            }
        }

        public void DeleteProduct(Guid id)
        {
            this._ticketRepository.Delete(_ticketRepository.Get(id));
        }

        public List<TicketProd> GetAllNotSoldProducts()
        {
            return this._ticketRepository.GetAllNotSold().ToList();
        }

        public List<TicketProd> GetAllProducts()
        {
            return this._ticketRepository.GetAll().ToList();
        }

        public List<TicketProd> GetByDateValid(DateTime selectedDate)
        {
            return this._ticketRepository.GetByDateValid(selectedDate).ToList();
        }

        public TicketProd GetDetailsForProduct(Guid? id)
        {
            return this._ticketRepository.Get(id);
        }

        public AddToShoppingCardDto GetShoppingCartInfo(Guid? id)
        {
            var ticket = this.GetDetailsForProduct(id);
            AddToShoppingCardDto model = new AddToShoppingCardDto
            {
                SelectedTicket = ticket,
                SelectedTicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdeteExistingProduct(TicketProd p)
        {
            this._ticketRepository.Update(p);
        }
    }
}
