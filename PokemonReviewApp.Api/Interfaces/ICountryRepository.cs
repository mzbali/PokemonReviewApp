using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface ICountryRepository
{
    public Task<ICollection<Country>> GetCountriesAsync();
    public Task<Country> GetCountryAsync(int countryId);
    public Task<ICollection<Owner>> GetOwnersByCountryAsync(int countryId);
    public Task<Country> GetCountryByOwnerAsync(int ownerId);
    public Task<bool> CountryExistsAsync(int countryId);
    public Task<bool> CountryExistsAsync(string countryName);
    public Task<bool> CreateCountryAsync(Country country);
    public Task<bool> SaveAsync();
}