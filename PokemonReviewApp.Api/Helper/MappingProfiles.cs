using AutoMapper;

namespace PokemonReviewApp.Api.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Models.Pokemon, Dto.PokemonDto>();
        CreateMap<Models.Category, Dto.CategoryDto>();
        CreateMap<Models.Country, Dto.CountryDto>();
    }
}
