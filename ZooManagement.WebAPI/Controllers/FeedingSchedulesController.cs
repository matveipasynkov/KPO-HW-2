using Microsoft.AspNetCore.Mvc;
using ZooManagement.Application.DTOs;
using ZooManagement.Application.Services;
using ZooManagement.Domain.Exceptions;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedingSchedulesController : ControllerBase
{
    private readonly IFeedingOrganizationService _feedingService;

    public FeedingSchedulesController(IFeedingOrganizationService feedingService)
    {
        _feedingService = feedingService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FeedingScheduleDto>), 200)]
    public async Task<ActionResult<IEnumerable<FeedingScheduleDto>>> GetAllSchedules()
    {
        var schedules = await _feedingService.GetAllSchedulesAsync();
        var dtos = schedules.Select(s => new FeedingScheduleDto
        {
            Id = s.Id,
            AnimalId = s.AnimalId,
            FeedingTime = s.FeedingTime,
            FoodType = s.FoodType.Value,
            IsCompleted = s.IsCompleted
        });
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FeedingScheduleDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<FeedingScheduleDto>> GetScheduleById(Guid id)
    {
        var schedule = await _feedingService.GetScheduleByIdAsync(id);
        if (schedule == null)
        {
            return NotFound($"Feeding schedule with ID {id} not found.");
        }
        var dto = new FeedingScheduleDto
        {
            Id = schedule.Id,
            AnimalId = schedule.AnimalId,
            FeedingTime = schedule.FeedingTime,
            FoodType = schedule.FoodType.Value,
            IsCompleted = schedule.IsCompleted
        };
        return Ok(dto);
    }

    [HttpGet("animal/{animalId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<FeedingScheduleDto>), 200)]
    public async Task<ActionResult<IEnumerable<FeedingScheduleDto>>> GetSchedulesForAnimal(Guid animalId)
    {
        var schedules = await _feedingService.GetSchedulesForAnimalAsync(animalId);
         var dtos = schedules.Select(s => new FeedingScheduleDto
        {
            Id = s.Id,
            AnimalId = s.AnimalId,
            FeedingTime = s.FeedingTime,
            FoodType = s.FoodType.Value,
            IsCompleted = s.IsCompleted
        });
        return Ok(dtos);
    }

    public record CreateFeedingScheduleRequest(Guid AnimalId, DateTime FeedingTime, string FoodType);

    [HttpPost]
    [ProducesResponseType(typeof(FeedingScheduleDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)] 
    public async Task<ActionResult<FeedingScheduleDto>> CreateSchedule([FromBody] CreateFeedingScheduleRequest request)
    {
        try
        {
            var scheduleId = await _feedingService.AddFeedingScheduleAsync(
                request.AnimalId,
                request.FeedingTime,
                new FoodType(request.FoodType)
            );

            var schedule = await _feedingService.GetScheduleByIdAsync(scheduleId);
            if (schedule == null) return StatusCode(500, "Failed to retrieve created schedule.");

             var dto = new FeedingScheduleDto
            {
                Id = schedule.Id,
                AnimalId = schedule.AnimalId,
                FeedingTime = schedule.FeedingTime,
                FoodType = schedule.FoodType.Value,
                IsCompleted = schedule.IsCompleted
            };

            return CreatedAtAction(nameof(GetScheduleById), new { id = schedule.Id }, dto);
        }
        catch (KeyNotFoundException knfex)
        {
            return NotFound(knfex.Message);
        }
        catch (DomainException dex)
        {
            return BadRequest(dex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred while creating the schedule.");
        }
    }

    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> MarkAsDone(Guid id)
    {
         try
        {
            await _feedingService.MarkScheduleAsDoneAsync(id);
            return Ok($"Feeding schedule {id} marked as completed.");
        }
        catch (KeyNotFoundException knfex)
        {
            return NotFound(knfex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

     public record ChangeFeedingScheduleRequest(DateTime FeedingTime, string FoodType);

    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChangeSchedule(Guid id, [FromBody] ChangeFeedingScheduleRequest request)
    {
         try
        {
            await _feedingService.ChangeScheduleAsync(id, request.FeedingTime, new FoodType(request.FoodType));
            return Ok($"Feeding schedule {id} updated.");
        }
        catch (KeyNotFoundException knfex)
        {
            return NotFound(knfex.Message);
        }
         catch (DomainException dex)
        {
            return BadRequest(dex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var existing = await _feedingService.GetScheduleByIdAsync(id);
        if (existing == null) return NotFound($"Schedule {id} not found.");

        await _feedingService.DeleteScheduleAsync(id);
        return NoContent();
    }
}
