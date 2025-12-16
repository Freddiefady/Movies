using Microsoft.EntityFrameworkCore;

namespace Movies.Services
{
    public class GenreService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        public Genre DeleteAsync(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChangesAsync();

            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public Task<bool> isVaildGenre(byte id)
        {
            return _context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre UpdateAsync(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChangesAsync();

            return genre;
        }
    }
}
