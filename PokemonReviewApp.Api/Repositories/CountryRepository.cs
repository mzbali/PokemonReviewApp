using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Api.Data;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Country>> GetCountriesAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country> GetCountryAsync(int countryId)
    {
        return await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryId);
    }

    public async Task<ICollection<Owner>> GetOwnersByCountryAsync(int countryId)
    {
        return await _context.Owners.Where(o => o.Country.Id == countryId).ToListAsync();
    }

    public async Task<Country> GetCountryByOwnerAsync(int ownerId)
    {
        return await _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
    }

    public async Task<bool> CountryExistsAsync(int countryId)
    {
        return await _context.Countries.AnyAsync(c => c.Id == countryId);
    }

    public Task<bool> CountryExistsAsync(string countryName)
    {
        return _context.Countries.AnyAsync(c => c.Name == countryName);
    }

    public async Task<bool> CreateCountryAsync(Country country)
    {
        await _context.Countries.AddAsync(country);
        return await SaveAsync();
    }
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> UpdateCountryAsync(Country country)
    {
        _context.Countries.Update(country);
        return await SaveAsync();
    }
    public async Task<bool> DeleteCountryAsync(Country country)
    {
        _context.Countries.Remove(country);
        return await SaveAsync();
    }
}
