using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Interfaces;

public interface ICategoryRepository
{
    public Task<ICollection<Category>> GetCategoriesAsync();
    public Task<Category> GetCategoryAsync(int categoryId);
    public Task<ICollection<Pokemon>> GetPokemonByCategoryAsync(int categoryId);
    public Task<bool> CategoryExistsAsync(int categoryId);
}
