using AutoMapper;

namespace Movies.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Movie, MovieDto>();
        }
    }
}
