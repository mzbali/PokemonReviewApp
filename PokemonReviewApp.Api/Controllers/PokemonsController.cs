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
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    public PokemonsController(IPokemonRepository repository, IReviewRepository reviewRepository, IMapper mapper)
    {
        _pokemonRepository = repository;
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))] // helps with Swagger documentation
    public async Task<IActionResult> GetPokemons()
    {
        var pokemons = await _pokemonRepository.GetPokemonsAsync();
        var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons);
        return Ok(pokemonsDto);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPokemon(int id)
    {
        var pokemon = await _pokemonRepository.GetPokemonAsync(id);
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
        var rating = await _pokemonRepository.GetPokemonRatingAsync(id);
        if (rating == 0)
        {
            return NotFound();
        }
        return Ok(rating);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonDto)
    {
        if (await _pokemonRepository.PokemonExistsAsync(pokemonDto.Name))
        {
            return BadRequest("Pokemon already exists");
        }
        var pokemon = _mapper.Map<Pokemon>(pokemonDto);
        if (!await _pokemonRepository.CreatePokemonAsync(ownerId, categoryId, pokemon))
        {
            return BadRequest("Failed to create Pokemon");
        }
        var pokemonToDto = _mapper.Map<PokemonDto>(pokemon);
        return CreatedAtAction(nameof(GetPokemon), new { id = pokemon.Id }, pokemonToDto);
    }
    [HttpPut("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400), ProducesResponseType(404), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> UpdatePokemon(int pokemonId, PokemonDto pokemonDto)
    {
        if (pokemonDto is null)
        {
            ModelState.AddModelError("", "Pokemon is empty");
            return BadRequest(ModelState);
        }
        if (pokemonId != pokemonDto.Id)
        {
            ModelState.AddModelError("", "Pokemon Id mismatch");
            return UnprocessableEntity(ModelState);
        }
        if (!await _pokemonRepository.PokemonExistsAsync(pokemonId))
        {
            return NotFound();
        }
        var pokemon = _mapper.Map<Pokemon>(pokemonDto);
        if (!await _pokemonRepository.UpdatePokemonAsync(pokemon))
        {
            ModelState.AddModelError("", "Something went wrong while updating the Pokemon");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }

    [HttpDelete("{pokemonId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404), ProducesResponseType(500)]
    public async Task<IActionResult> DeletePokemon(int pokemonId)
    {
        var pokemon = await _pokemonRepository.GetPokemonAsync(pokemonId);
        if (pokemon == null)
        {
            return NotFound();
        }
        if (!await _pokemonRepository.DeletePokemonAsync(pokemon))
        {
            ModelState.AddModelError("", "Failed to delete the Pokemon");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }

}