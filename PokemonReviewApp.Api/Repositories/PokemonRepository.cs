using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Api.Data;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;
namespace PokemonReviewApp.Api.Repositories;

public class PokemonRepository : IPokemonRepository
{
    private readonly AppDbContext _context;
    public PokemonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pokemon>> GetPokemonsAsync()
    {
        return await _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
    }
    public async Task<Pokemon> GetPokemonAsync(int id)
    {
        return await _context.Pokemon.FindAsync(id);
    }

    public async Task<Pokemon> GetPokemonAsync(string name)
    {
        return await _context.Pokemon.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<decimal> GetPokemonRatingAsync(int id)
    {
        var reviews = _context.Reviews.Where(r => r.Pokemon.Id == id);

        if (!await reviews.AnyAsync())
        {
            return 0;
        }

        // TODO: add navigation property to Review for PokemonId and ReviewerId
        return (decimal)await reviews.AverageAsync(r => r.Rating);
    }

    public Task<bool> PokemonExistsAsync(int id)
    {
        return _context.Pokemon.AnyAsync(p => p.Id == id);
    }

    public Task<bool> PokemonExistsAsync(string name)
    {
        return _context.Pokemon.AnyAsync(p => p.Name == name);
    }
    public async Task<bool> CreatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon)
    {
        var owner = await _context.Owners.FindAsync(ownerId);
        var category = await _context.Categories.FindAsync(categoryId);
        if (owner == null || category == null)
        {
            return false;
        }
        var pokemonCategory = new PokemonCategory
        {
            Pokemon = pokemon,
            Category = category
        };
        var pokemonOwner = new PokemonOwner
        {
            Pokemon = pokemon,
            Owner = owner
        };
        await _context.PokemonCategories.AddAsync(pokemonCategory);
        await _context.PokemonOwners.AddAsync(pokemonOwner);
        await _context.Pokemon.AddAsync(pokemon);
        return await SaveAsync();
    }
    public async Task<bool> UpdatePokemonAsync(Pokemon pokemon)
    {
        _context.Pokemon.Update(pokemon);
        return await SaveAsync();
    }
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeletePokemonAsync(Pokemon pokemon)
    {
        _context.Pokemon.Remove(pokemon);
        return await SaveAsync();
    }
}
