namespace Movies.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
        public double Rate { get; set; }
        public byte[] Poster { get; set; }

        public byte GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}
