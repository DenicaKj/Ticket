using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticket.Domain.DomainModels;
using Ticket.Repository.Interface;
using Ticket.Service.Interface;

namespace Ticket.Service.Implementation
{
    public class MovieService:IMovieService
    {
        private readonly IRepository<Movie> _movieRepository;
    
        public MovieService(IRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void CreateNewMovie(Movie p)
        {
            this._movieRepository.Insert(p);
        }

        public void DeleteMovie(Guid id)
        {
            this._movieRepository.Delete(_movieRepository.Get(id));
        }

        public List<Movie> GetAllMovies()
        {
            return this._movieRepository.GetAll().ToList();
        }

        public Movie GetDetailsForMovie(Guid? id)
        {
            return this._movieRepository.Get(id);
        }


        public void UpdeteExistingMovie(Movie p)
        {
            this._movieRepository.Update(p);
        }
    }
}
