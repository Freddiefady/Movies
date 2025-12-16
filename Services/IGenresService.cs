namespace Movies.Services
{
    public interface IGenresService
    {
		// Retrieves all genres asynchronously.
		Task<IEnumerable<Genre>> GetAll();

		// Retrieves a genre by its ID asynchronously.
		Task<Genre?> GetByIdAsync(byte id);

		// Adds a new genre asynchronously.
		Task<Genre> AddAsync(Genre genre);

		// Updates an existing genre asynchronously.
		Genre UpdateAsync(Genre genre);

		// Deletes a genre by its ID asynchronously.
		Genre DeleteAsync(Genre genre);

        Task<bool> isVaildGenre(byte id);
	}
}
