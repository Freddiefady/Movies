using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.Services;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        private readonly IMapper _mapper;

		private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };

        private readonly long _maxPosterSize = 1048576; // 1 MB

        public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
        {
            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Eager loading Genre data with orderByDesc Movies
            var movies = await _moviesService.GetAll();
            IEnumerable<MovieDto> dtoMovies = getMovieDetails(movies);

            return Ok(dtoMovies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> show(int id)
        {
            var movie = await _moviesService.GetByIdAsync(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpGet("GetGenreId")]
        public async Task<IActionResult> GetByGenreId(byte genreId)
        {
            var movie = await _moviesService.GetAll(genreId);

			if (movie == null)
                return NotFound();

			IEnumerable<MovieDto> dtoMovies = getMovieDetails(movie);


            return Ok(dtoMovies);
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required.");

			// Validate Poster
			if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg, .png, .jpeg files are allowed.");

            if (dto.Poster.Length > _maxPosterSize)
                return BadRequest("Poster size cannot exceed 1 MB.");

            // Validate GenreId
            var isValid = await _genresService.isVaildGenre(dto.GenreId);

            if (!isValid)
                return BadRequest("Invalid Genre Id.");

            using var dataStream = new MemoryStream();

            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                Rate = dto.Rate,
                Poster = dataStream.ToArray(),
                GenreId = dto.GenreId
            };

            await _moviesService.AddAsync(movie);

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDto dto)
        {
            var movie = await _moviesService.GetByIdAsync(id);

			if (movie == null)
                return NotFound($"No Movie was found with ID: {id}");

			// Handle Poster update
			if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .jpg, .png, .jpeg files are allowed.");

                if (dto.Poster.Length > _maxPosterSize)
                    return BadRequest("Poster size cannot exceed 1 MB.");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            // Validate GenreId
            var isValid = await _genresService.isVaildGenre(dto.GenreId);
            if (!isValid)
                return BadRequest("Invalid Genre Id.");

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.Rate = dto.Rate;
            movie.GenreId = dto.GenreId;

            _moviesService.UpdateAsync(movie);
            return Ok(movie);
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var movie = await _moviesService.GetByIdAsync(id);

            if (movie == null)
                return NotFound($"No Movie was found with ID: {id}");

            _moviesService.DeleteAsync(movie);
            return NoContent();
		}

		private IEnumerable<MovieDto> getMovieDetails(IEnumerable<Movie> movies)
		{
			return _mapper.Map<IEnumerable<MovieDto>>(movies);
		}
	}
}
