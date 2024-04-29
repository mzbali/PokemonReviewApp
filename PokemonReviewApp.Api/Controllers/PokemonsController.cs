using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonsController : ControllerBase
{
    private readonly IPokemonRepository _repository;
    private readonly IMapper _mapper;
    public PokemonsController(IPokemonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))] // helps with Swagger documentation
    public async Task<IActionResult> GetPokemons()
    {
        var pokemons = await _repository.GetPokemonsAsync();
        var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons);
        return Ok(pokemonsDto);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPokemon(int id)
    {
        var pokemon = await _repository.GetPokemonAsync(id);
        if (pokemon == null)
        {
            return NotFound();
        }
        var pokemonDto = _mapper.Map<PokemonDto>(pokemon);
        return Ok(pokemonDto);
    }

    [HttpGet("{id}/rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPokemonRating(int id)
    {
        var rating = await _repository.GetPokemonRatingAsync(id);
        if (rating == 0)
        {
            return NotFound();
        }
        return Ok(rating);
    }
}