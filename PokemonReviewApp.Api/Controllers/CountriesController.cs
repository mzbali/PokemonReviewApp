using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;

namespace PokemonReviewApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public CountriesController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<CountryDto>>> GetCountries()
    {
        var countries = await _countryRepository.GetCountriesAsync();
        return Ok(_mapper.Map<ICollection<CountryDto>>(countries));
    }

    [HttpGet("{countryId}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int countryId)
    {
        var country = await _countryRepository.GetCountryAsync(countryId);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<CountryDto>(country));
    }

    [HttpGet("/owners/{ownerId}")]
    public async Task<ActionResult<CountryDto>> GetCountryOfAnOwner(int ownerId)
    {
        var country = await _countryRepository.GetCountryByOwnerAsync(ownerId);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<CountryDto>(country));
    }
}