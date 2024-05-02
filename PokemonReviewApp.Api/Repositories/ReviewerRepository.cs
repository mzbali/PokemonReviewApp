using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Api.Data;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Repositories;

public class ReviewerRepository : IReviewerRepository
{
    private readonly AppDbContext _context;

    public ReviewerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reviewer>> GetReviewersAsync()
    {
        return await _context.Reviewers.ToListAsync();
    }
    public async Task<Reviewer> GetReviewerAsync(int reviewerId)
    {
        return await _context.Reviewers.FirstOrDefaultAsync(r => r.Id == reviewerId);
    }

    public async Task<ICollection<Review>> GetReviewsByAReviewerAsync(int reviewerId)
    {
        return await _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToListAsync();
    }

    public Task<bool> ReviewerExistsAsync(int reviewerId)
    {
        return _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
    }

    public Task<bool> ReviewerExistsAsync(string lastName)
    {
        return _context.Reviewers.AnyAsync(r => r.LastName == lastName);
    }

    public async Task<bool> CreateReviewerAsync(Reviewer reviewer)
    {
        await _context.Reviewers.AddAsync(reviewer);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> UpdateReviewerAsync(Reviewer reviewer)
    {
        _context.Reviewers.Update(reviewer);
        return await SaveAsync();
    }
}
