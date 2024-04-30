using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface IReviewRepository
{
    public Task<IEnumerable<Review>> GetReviewsAsync();
    public Task<Review> GetReviewAsync(int reviewId);
    public Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId);
    public Task<bool> ReviewExistsAsync(int reviewId);
}
