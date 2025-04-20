using Microsoft.AspNetCore.Mvc;
using ZooManagement.Application.DTOs;
using ZooManagement.Application.Services;
using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Exceptions;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnclosuresController : ControllerBase
{
    private readonly IEnclosureService _enclosureService;

    public EnclosuresController(IEnclosureService enclosureService)
    {
        _enclosureService = enclosureService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EnclosureDto>), 200)]
    public async Task<ActionResult<IEnumerable<EnclosureDto>>> GetAllEnclosures()
    {
        var enclosures = await _enclosureService.GetAllEnclosuresAsync();
        var enclosureDtos = enclosures.Select(e => new EnclosureDto
        {
            Id = e.Id,
            Type = e.Type.ToString(),
            Size = e.Size,
            MaxCapacity = e.MaxCapacity,
            CurrentAnimalCount = e.CurrentAnimalCount,
            AnimalIds = e.AnimalIds
        });
        return Ok(enclosureDtos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EnclosureDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<EnclosureDto>> GetEnclosureById(Guid id)
    {
        var enclosure = await _enclosureService.GetEnclosureByIdAsync(id);
        if (enclosure == null)
        {
            return NotFound($"Enclosure with ID {id} not found.");
        }
        var enclosureDto = new EnclosureDto
        {
            Id = enclosure.Id,
            Type = enclosure.Type.ToString(),
            Size = enclosure.Size,
            MaxCapacity = enclosure.MaxCapacity,
            CurrentAnimalCount = enclosure.CurrentAnimalCount,
            AnimalIds = enclosure.AnimalIds
        };
        return Ok(enclosureDto);
    }

    public record CreateEnclosureRequest(EnclosureType Type, int Size, int MaxCapacity);

    [HttpPost]
    [ProducesResponseType(typeof(EnclosureDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<EnclosureDto>> CreateEnclosure([FromBody] CreateEnclosureRequest request)
    {
        try
        {
            var enclosure = Enclosure.Create(request.Type, request.Size, request.MaxCapacity);
            await _enclosureService.AddEnclosureAsync(enclosure);

            var enclosureDto = new EnclosureDto
            {
                Id = enclosure.Id,
                Type = enclosure.Type.ToString(),
                Size = enclosure.Size,
                MaxCapacity = enclosure.MaxCapacity,
                CurrentAnimalCount = enclosure.CurrentAnimalCount,
                AnimalIds = enclosure.AnimalIds
            };

            return CreatedAtAction(nameof(GetEnclosureById), new { id = enclosure.Id }, enclosureDto);
        }
        catch (DomainException dex)
        {
            return BadRequest($"Validation failed: {dex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred while creating the enclosure.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteEnclosure(Guid id)
    {
        try
        {
             var existing = await _enclosureService.GetEnclosureByIdAsync(id);
             if (existing == null)
             {
                 return NotFound($"Enclosure with ID {id} not found.");
             }

            await _enclosureService.DeleteEnclosureAsync(id);
            return NoContent();
        }
        catch (DomainException dex)
        {
            return BadRequest(dex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occurred while deleting the enclosure.");
        }
    }
}
