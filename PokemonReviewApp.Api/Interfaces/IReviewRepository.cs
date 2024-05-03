using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface IReviewRepository
{
    public Task<IEnumerable<Review>> GetReviewsAsync();
    public Task<Review> GetReviewAsync(int reviewId);
    public Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId);
    public Task<bool> ReviewExistsAsync(int reviewId);
    public Task<bool> ReviewExistsAsync(string reviewTitle);
    public Task<bool> CreateReviewAsync(Review review);
    public Task<bool> UpdateReviewAsync(Review review);
    public Task<bool> DeleteReviewAsync(Review review);
    public Task<bool> SaveAsync();
}
