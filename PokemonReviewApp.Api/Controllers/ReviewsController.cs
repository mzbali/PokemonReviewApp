using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    public ReviewsController(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public async Task<IActionResult> GetReviews()
    {
        var reviews = await _reviewRepository.GetReviewsAsync();
        return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetReview(int reviewId)
    {
        var review = await _reviewRepository.GetReviewAsync(reviewId);
        if (review == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<ReviewDto>(review));
    }

    [HttpGet("pokemon/{pokemonId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetReviewsOfAPokemon(int pokemonId)
    {
        var reviews = await _reviewRepository.GetReviewsOfAPokemonAsync(pokemonId);
        if (reviews == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }
}
