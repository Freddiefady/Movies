using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Movies.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GenresController : ControllerBase
	{
        private readonly Models.ApplicationDbContext _context;

		public GenresController(Models.ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var genres = await _context.Genres.ToList();
			return Ok(genres);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Show(byte id)
		{
			var genre = await _context.Genres.Find(id);

			if (genre.IsNull()) return NotFound();

			return Ok(genre);
		}

		[HttpPost]
		public async Task<IActionResult> Store([FromBody] GenreDto genre)
		{
			var genre = new Genre { Name = dto.Name };

			await _context.Add(genre);
			_context.SaveChanges();

			return Ok(genre);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] GenreDto dto)
		{
			var genre = await _context.Genres.SingleOrDefaultAsync(genre => genre.Id == id);

			if (genre == null) return NotFound($"No Genre was Found with ID: {id}");
			
			genre.Name = dto.Name;
			_context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(byte id)
		{
			var genre = await _context.Genres.SingleOrDefaultAsync(genre => genre.Id == id);

			if (genre == null) return NotFound($"No Genre was Found with ID: {id}");

			_context.Remove(genre);
			_context.SaveChangesAsync();

			return NoContent();
		}
	}
}