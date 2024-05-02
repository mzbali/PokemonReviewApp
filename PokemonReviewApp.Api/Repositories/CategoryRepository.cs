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

    public async Task<bool> CreateCategoryAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        return await SaveAsync();
    }
    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<bool> CategoryExistsAsync(string categoryName)
    {
        return _context.Categories.AnyAsync(c => c.Name == categoryName);
    }
    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        return await SaveAsync();
    }
}