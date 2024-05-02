using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Api.Data;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly AppDbContext _context;

    public OwnerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Owner>> GetOwnersAsync()
    {
        return await _context.Owners.ToListAsync();
    }
    public async Task<Owner> GetOwnerAsync(int ownerId)
    {
        return await _context.Owners.FirstOrDefaultAsync(o => o.Id == ownerId);
    }

    public async Task<ICollection<Pokemon>> GetPokemonByOwnerAsync(int ownerId)
    {
        return await _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToListAsync();
    }

    public async Task<ICollection<Owner>> GetOwnerByPokemonAsync(int pokemonId)
    {
        return await _context.PokemonOwners.Where(po => po.PokemonId == pokemonId).Select(po => po.Owner).ToListAsync();
    }
    public Task<bool> OwnerExistsAsync(int ownerId)
    {
        return _context.Owners.AnyAsync(o => o.Id == ownerId);
    }

    public Task<bool> OwnerExistsAsync(string lastName)
    {
        return _context.Owners.AnyAsync(o => o.LastName == lastName);
    }

    public async Task<bool> AddOwnerAsync(Owner owner)
    {
        await _context.Owners.AddAsync(owner);
        return await SaveAsync();
    }
    public async Task<bool> UpdateOwnerAsync(Owner owner)
    {
        _context.Owners.Update(owner);
        return await SaveAsync();
    }
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}

