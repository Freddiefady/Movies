using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAll(byte? gerneId = 0);

        Task<Movie?> GetByIdAsync(int id);

        Task<Movie> AddAsync(Movie movie);

        Movie UpdateAsync(Movie movie);

        Movie DeleteAsync(Movie movie);
    }
}
