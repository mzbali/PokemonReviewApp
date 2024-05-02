using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface IPokemonRepository
{
    public Task<IEnumerable<Pokemon>> GetPokemonsAsync();
    public Task<Pokemon> GetPokemonAsync(int id);
    public Task<Pokemon> GetPokemonAsync(string name);
    public Task<decimal> GetPokemonRatingAsync(int id);
    public Task<bool> PokemonExistsAsync(int id);
    public Task<bool> PokemonExistsAsync(string name);
    public Task<bool> CreatePokemonAsync(int pokemonId, int categoryId, Pokemon pokemon);
    public Task<bool> UpdatePokemonAsync(Pokemon pokemon);
    public Task<bool> SaveAsync();
}
