using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface IOwnerRepository
{
    public Task<IEnumerable<Owner>> GetOwnersAsync();
    public Task<Owner> GetOwnerAsync(int ownerId);
    public Task<ICollection<Pokemon>> GetPokemonByOwnerAsync(int ownerId);
    public Task<ICollection<Owner>> GetOwnerByPokemonAsync(int pokemonId);
    public Task<bool> OwnerExistsAsync(int ownerId);
    public Task<bool> OwnerExistsAsync(string lastName);
    public Task<bool> AddOwnerAsync(Owner owner);
    public Task<bool> SaveAsync();
}
