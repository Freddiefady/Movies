using System;

public class MovieDto
{
	[MaxLength(250)]
	public string Title { get; set; }

	[MaxLength(2500)]
	public string Description { get; set; }

	public DateTime ReleaseDate { get; set; }
	public double Rate { get; set; }
	public IFormFile? Poster { get; set; }

	public byte GenreId { get; set; }
}
