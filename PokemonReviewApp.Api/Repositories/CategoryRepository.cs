using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Api.Data;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Repositories;

public class CategoryRepositpry : ICategoryRepository
{
    private readonly AppDbContext _context;
    public CategoryRepositpry(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ICollection<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryAsync(int categoryId)
    {
        return await _context.Categories.FindAsync(categoryId);
    }

    public async Task<ICollection<Pokemon>> GetPokemonByCategoryAsync(int categoryId)
    {
        return await _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(p => p.Pokemon).ToListAsync();
    }
    public Task<bool> CategoryExistsAsync(int categoryId)
    {
        return _context.Categories.AnyAsync(c => c.Id == categoryId);
    }
}