using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

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
    [ProducesResponseType(200, Type = typeof(ICollection<CountryDto>))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ICollection<CountryDto>>> GetCountries()
    {
        var countries = await _countryRepository.GetCountriesAsync();
        return Ok(_mapper.Map<ICollection<CountryDto>>(countries));
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(CountryDto))]
    [ProducesResponseType(404)]
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
    [ProducesResponseType(200, Type = typeof(CountryDto))]
    [ProducesResponseType(404)]
    public async Task<ActionResult<CountryDto>> GetCountryOfAnOwner(int ownerId)
    {
        var country = await _countryRepository.GetCountryByOwnerAsync(ownerId);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<CountryDto>(country));
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CountryDto))]
    [ProducesResponseType(422), ProducesResponseType(400), ProducesResponseType(500)]
    public async Task<ActionResult<CountryDto>> CreateCountry(CountryDto countryDto)
    {
        if (await _countryRepository.CountryExistsAsync(countryDto.Name))
        {
            ModelState.AddModelError("Name", "Country already exists");
            return StatusCode(422, ModelState);
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var country = _mapper.Map<Country>(countryDto);

        if (!await _countryRepository.CreateCountryAsync(country))
        {
            ModelState.AddModelError("", "something went wrong while saving the country");
            return StatusCode(500, ModelState);
        }
        var countryToDto = _mapper.Map<CountryDto>(country);
        return CreatedAtAction(nameof(GetCountry), new { countryId = country.Id }, countryToDto);
    }
    [HttpPut("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400), ProducesResponseType(404), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> UpdateCountry(int countryId, CountryDto countryDto)
    {
        if (countryDto is null)
        {
            ModelState.AddModelError("", "Country object is null");
            return BadRequest(ModelState);
        }
        if (countryId != countryDto.Id)
        {
            ModelState.AddModelError("Id", "Country Id mismatch");
            return UnprocessableEntity(ModelState);
        }
        if (!await _countryRepository.CountryExistsAsync(countryId))
        {
            return NotFound("Country not found");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var country = _mapper.Map<Country>(countryDto);
        if (!await _countryRepository.UpdateCountryAsync(country))
        {
            ModelState.AddModelError("", "Failed to update the country");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
    [HttpDelete("{countryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404), ProducesResponseType(500)]
    public async Task<IActionResult> DeleteCountry(int countryId)
    {
        var country = await _countryRepository.GetCountryAsync(countryId);
        if (country == null)
        {
            return NotFound();
        }
        if (!await _countryRepository.DeleteCountryAsync(country))
        {
            ModelState.AddModelError("", "Failed to delete the country");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
}