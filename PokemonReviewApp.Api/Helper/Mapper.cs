using AutoMapper;

namespace PokemonReviewApp.Api.Helper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Models.Pokemon, Dto.PokemonDto>();
    }
}
