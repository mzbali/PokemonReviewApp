using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();
        var categoriesDto = _mapper.Map<ICollection<CategoryDto>>(categories);
        return Ok(categoriesDto);
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCategory(int categoryId)
    {
        if (!await _categoryRepository.CategoryExistsAsync(categoryId))
        {
            return NotFound();
        }

        var category = await _categoryRepository.GetCategoryAsync(categoryId);
        var categoryDto = _mapper.Map<CategoryDto>(category);
        return Ok(categoryDto);
    }

    [HttpGet("{categoryId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPokemonByCategory(int categoryId)
    {
        if (!await _categoryRepository.CategoryExistsAsync(categoryId))
        {
            return NotFound();
        }

        var pokemon = await _categoryRepository.GetPokemonByCategoryAsync(categoryId);
        var pokemonDto = _mapper.Map<ICollection<PokemonDto>>(pokemon);
        return Ok(pokemonDto);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Category))]
    [ProducesResponseType(400), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
    {
        if (await _categoryRepository.CategoryExistsAsync(categoryDto.Name))
        {
            ModelState.AddModelError("Name", "Category name already exists");
            return StatusCode(422, ModelState);
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = _mapper.Map<Category>(categoryDto);
        if (!await _categoryRepository.CreateCategoryAsync(category))
        {
            ModelState.AddModelError("", $"Failed to save the category: {categoryDto.Name}");
            return StatusCode(500, ModelState);
        }

        var categoryDtoToReturn = _mapper.Map<CategoryDto>(category);
        return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, categoryDtoToReturn);
    }
}
