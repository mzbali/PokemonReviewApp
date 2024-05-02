using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Api.Dto;
using PokemonReviewApp.Api.Interfaces;
using PokemonReviewApp.Api.Models;

namespace PokemonReviewApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewersController : ControllerBase
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;
    public ReviewersController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
    public async Task<IActionResult> GetReviewers()
    {
        var reviewers = await _reviewerRepository.GetReviewersAsync();
        return Ok(_mapper.Map<IEnumerable<ReviewerDto>>(reviewers));
    }

    [HttpGet("{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetReviewer(int reviewerId)
    {
        var reviewer = await _reviewerRepository.GetReviewerAsync(reviewerId);
        if (reviewer == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<ReviewerDto>(reviewer));
    }

    [HttpGet("{reviewerId}/reviews")]
    [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetReviewsByAReviewer(int reviewerId)
    {
        var reviewerExists = await _reviewerRepository.ReviewerExistsAsync(reviewerId);
        if (!reviewerExists)
        {
            return NotFound();
        }
        var reviews = await _reviewerRepository.GetReviewsByAReviewerAsync(reviewerId);
        return Ok(_mapper.Map<ICollection<ReviewDto>>(reviews));
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Reviewer))]
    [ProducesResponseType(400), ProducesResponseType(422), ProducesResponseType(500)]
    public async Task<IActionResult> CreateReviewer(ReviewerDto reviewerDto)
    {
        if (await _reviewerRepository.ReviewerExistsAsync(reviewerDto.LastName))
        {
            ModelState.AddModelError("LastName", "Reviewer already exists");
            return StatusCode(422, ModelState);
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var reviewer = _mapper.Map<Reviewer>(reviewerDto);
        if (!await _reviewerRepository.CreateReviewerAsync(reviewer))
        {
            ModelState.AddModelError("", $"Failed to create reviewer: {reviewerDto.LastName}");
            return StatusCode(500, ModelState);
        }
        var reviewerDtoToReturn = _mapper.Map<ReviewerDto>(reviewer);
        return CreatedAtAction(nameof(GetReviewer), new { reviewerId = reviewer.Id }, reviewerDtoToReturn);
    }
}
