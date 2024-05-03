using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;
    public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _countryRepository = countryRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    public async Task<IActionResult> GetOwners()
    {
        var owners = await _ownerRepository.GetOwnersAsync();
        return Ok(_mapper.Map<IEnumerable<OwnerDto>>(owners));
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOwner(int ownerId)
    {
        var owner = await _ownerRepository.GetOwnerAsync(ownerId);
        if (owner == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<OwnerDto>(owner));
    }

    [HttpGet("{ownerId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPokemonByAnOwner(int ownerId)
    {
        var pokemon = await _ownerRepository.GetPokemonByOwnerAsync(ownerId);
        if (pokemon == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<PokemonDto>>(pokemon));
    }

    [HttpGet("pokemon/{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOwnerByPokemon(int pokemonId)
    {
        var owners = await _ownerRepository.GetOwnerByPokemonAsync(pokemonId);
        if (owners == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<OwnerDto>>(owners));
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Owner))]
    [ProducesResponseType(422), ProducesResponseType(400), ProducesResponseType(500)]
    public async Task<IActionResult> CreateOwner([FromQuery] int countryId, OwnerDto ownerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var owner = _mapper.Map<Owner>(ownerDto);
        var country = await _countryRepository.GetCountryAsync(countryId);
        if (country == null)
        {
            ModelState.AddModelError("CountryId", "Country not found");
            return StatusCode(400, ModelState);
        }
        owner.Country = country;

        if (!await _ownerRepository.AddOwnerAsync(owner))
        {
            ModelState.AddModelError("", "Something went wrong saving the owner");
            return StatusCode(500, ModelState);
        }
        var ownerToDto = _mapper.Map<OwnerDto>(owner);
        return CreatedAtAction(nameof(GetOwner), new { ownerId = owner.Id }, ownerToDto);
    }
    [HttpPut("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400), ProducesResponseType(404), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> UpdateOwner(int ownerId, OwnerDto ownerDto)
    {
        if (ownerDto is null)
        {
            return BadRequest("Owner data must not be null");
        }
        if (ownerId != ownerDto.Id)
        {
            ModelState.AddModelError("Id", "Owner Id mismatch");
            return UnprocessableEntity(ModelState); // 422 Unprocessable Entity
        }
        if (!await _ownerRepository.OwnerExistsAsync(ownerId))
        {
            return NotFound("Owner with the specified ID not found");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var owner = _mapper.Map<Owner>(ownerDto);
        if (!await _ownerRepository.UpdateOwnerAsync(owner))
        {
            ModelState.AddModelError("", "Something went wrong while updating the owner");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
    [HttpDelete("{ownerId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404), ProducesResponseType(500)]
    public async Task<IActionResult> DeleteOwner(int ownerId)
    {
        var owner = await _ownerRepository.GetOwnerAsync(ownerId);
        if (owner == null)
        {
            return NotFound();
        }
        if (!await _ownerRepository.DeleteOwnerAsync(owner))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the owner");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
}
