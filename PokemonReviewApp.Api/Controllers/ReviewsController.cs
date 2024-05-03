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
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;
    public ReviewsController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _reviewerRepository = reviewerRepository;
        _pokemonRepository = pokemonRepository;
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

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Review))]
    [ProducesResponseType(400), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> CreateReview([FromQuery] int pokeId, [FromQuery] int reviewerId, [FromBody] ReviewDto reviewDto)
    {
        if (await _reviewRepository.ReviewExistsAsync(reviewDto.Title))
        {
            ModelState.AddModelError("", "Review already exists");
            return StatusCode(422, ModelState);
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var pokemon = await _pokemonRepository.GetPokemonAsync(pokeId);
        if (pokemon == null)
        {
            ModelState.AddModelError("Pokemon", "Pokemon does not exist");
            return StatusCode(422, ModelState);
        }
        var reviewer = await _reviewerRepository.GetReviewerAsync(reviewerId);
        if (reviewer == null)
        {
            ModelState.AddModelError("Reviewer", "Reviewer does not exist");
            return StatusCode(422, ModelState);
        }
        var review = _mapper.Map<Review>(reviewDto);
        review.Pokemon = pokemon;
        review.Reviewer = reviewer;
        if (!await _reviewRepository.CreateReviewAsync(review))
        {
            return StatusCode(500, "Something went wrong while saving the review.");
        }
        var reviewDtoToReturn = _mapper.Map<ReviewDto>(review);
        return CreatedAtAction(nameof(GetReview), new { reviewId = review.Id }, reviewDtoToReturn);
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400), ProducesResponseType(404), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> UpdateReview(int reviewId, ReviewDto reviewDto)
    {
        if (reviewDto is null)
        {
            ModelState.AddModelError("", "Review object is null");
            return BadRequest(ModelState);
        }
        if (reviewId != reviewDto.Id)
        {
            ModelState.AddModelError("", "Review Id mismatch");
            return StatusCode(422, ModelState);
        }
        if (!await _reviewRepository.ReviewExistsAsync(reviewId))
        {
            return NotFound();
        }
        var review = _mapper.Map<Review>(reviewDto);
        if (!await _reviewRepository.UpdateReviewAsync(review))
        {
            ModelState.AddModelError("", "Something went wrong while updating the review");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
    [HttpDelete("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404), ProducesResponseType(500)]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        var review = await _reviewRepository.GetReviewAsync(reviewId);
        if (review is null)
        {
            return NotFound();
        }
        if (!await _reviewRepository.DeleteReviewAsync(review))
        {
            ModelState.AddModelError("", "Failed to delete the review");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
}
