using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface IReviewerRepository
{
    public Task<IEnumerable<Reviewer>> GetReviewersAsync();
    public Task<Reviewer> GetReviewerAsync(int reviewerId);
    public Task<ICollection<Review>> GetReviewsByAReviewerAsync(int reviewerId);
    public Task<bool> ReviewerExistsAsync(int reviewerId);
    public Task<bool> ReviewerExistsAsync(string lastName);
    public Task<bool> CreateReviewerAsync(Reviewer reviewer);
    public Task<bool> SaveAsync();
}
