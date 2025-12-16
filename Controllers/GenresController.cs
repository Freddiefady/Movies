using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Services;

namespace Movies.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GenresController : ControllerBase
	{
        private readonly IGenresService _genresService;

		public GenresController(IGenresService genresService)
		{
			_genresService = genresService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var genres = await _genresService.GetAll();
			return Ok(genres);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Show(byte id)
		{
			var genre = await _genresService.GetByIdAsync(id);

			if (genre == null) return NotFound();

			return Ok(genre);
		}

		[HttpPost]
		public async Task<IActionResult> Store([FromBody] GenreDto dto)
		{
			var genre = new Genre { Name = dto.Name };
			await _genresService.AddAsync(genre);

			return Ok(genre);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(byte id, [FromBody] GenreDto dto)
		{
			var genre = await _genresService.GetByIdAsync(id);

			if (genre == null)
                return NotFound($"No Genre was Found with ID: {id}");

			genre.Name = dto.Name;
			_genresService.UpdateAsync(genre);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(byte id)
		{
			var genre = await _genresService.GetByIdAsync(id);

			if (genre == null) return NotFound($"No Genre was Found with ID: {id}");

			_genresService.DeleteAsync(genre);

			return NoContent();
		}
	}
}
