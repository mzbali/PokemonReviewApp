using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
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
}
