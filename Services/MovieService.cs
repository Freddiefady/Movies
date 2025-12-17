using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Services
{
    public class MovieService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
             await _context.SaveChangesAsync();

            return movie;
        }

        public Movie DeleteAsync(Movie movie)
        {
			_context.Movies.Remove(movie);
			_context.SaveChangesAsync();

			return movie;
		}

        public async Task<IEnumerable<Movie>> GetAll(byte? gerneId = 0)
        {
            return await _context.Movies
                .Where(m => m.GenreId == gerneId || gerneId == 0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
			return await _context.Movies
				.Include(m => m.Genre)
				.SingleOrDefaultAsync(m => m.Id == id);
		}

        public Movie UpdateAsync(Movie movie)
        {
			_context.Update(movie);
			_context.SaveChangesAsync();

            return movie;
		}
    }
}
