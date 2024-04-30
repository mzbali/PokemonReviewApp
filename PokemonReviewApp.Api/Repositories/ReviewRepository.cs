using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Api.Data;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;
    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Review>> GetReviewsAsync()
    {
        return await _context.Reviews.ToListAsync();
    }
    public async Task<Review> GetReviewAsync(int reviewId)
    {
        return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
    }

    public async Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId)
    {
        return await _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToListAsync();
    }

    public Task<bool> ReviewExistsAsync(int reviewId)
    {
        return _context.Reviews.AnyAsync(r => r.Id == reviewId);
    }
}
