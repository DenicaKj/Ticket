using System;
using System.Collections.Generic;
using System.Text;
using Ticket.Domain.DomainModels;

namespace Ticket.Service.Interface
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetDetailsForMovie(Guid? id);
        void CreateNewMovie(Movie p);
        void UpdeteExistingMovie(Movie p);
        void DeleteMovie(Guid id);
    }
}
