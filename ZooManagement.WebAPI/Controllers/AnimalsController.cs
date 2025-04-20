using Microsoft.AspNetCore.Mvc;
using ZooManagement.Application.DTOs;
using ZooManagement.Application.Services;
using ZooManagement.Domain.Entities;
using ZooManagement.Domain.ValueObjects; 
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;
    private readonly IAnimalTransferService _transferService;

    public AnimalsController(IAnimalService animalService, IAnimalTransferService transferService)
    {
        _animalService = animalService;
        _transferService = transferService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AnimalDto>), 200)]
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAllAnimals()
    {
        var animals = await _animalService.GetAllAnimalsAsync();
        var animalDtos = animals.Select(a => new AnimalDto
        {
            Id = a.Id,
            Name = a.Name.Value,
            Species = a.Species.Value,
            DateOfBirth = a.DateOfBirth,
            Sex = a.Sex.ToString(),
            FavoriteFood = a.FavoriteFood.Value,
            Status = a.Status.ToString(),
            CurrentEnclosureId = a.CurrentEnclosureId
        });
        return Ok(animalDtos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AnimalDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<AnimalDto>> GetAnimalById(Guid id)
    {
        var animal = await _animalService.GetAnimalByIdAsync(id);
        if (animal == null)
        {
            return NotFound($"Animal with ID {id} not found.");
        }
        var animalDto = new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name.Value,
            Species = animal.Species.Value,
            DateOfBirth = animal.DateOfBirth,
            Sex = animal.Sex.ToString(),
            FavoriteFood = animal.FavoriteFood.Value,
            Status = animal.Status.ToString(),
            CurrentEnclosureId = animal.CurrentEnclosureId
        };
        return Ok(animalDto);
    }

    public record CreateAnimalRequest(string Name, string Species, DateTime DateOfBirth, Sex Sex, string FavoriteFood);

    [HttpPost]
    [ProducesResponseType(typeof(AnimalDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AnimalDto>> CreateAnimal([FromBody] CreateAnimalRequest request)
    {
        try
        {
            var animal = Animal.Create(
                new Species(request.Species),
                new AnimalName(request.Name),
                request.DateOfBirth,
                request.Sex,
                new FoodType(request.FavoriteFood)
            );

            await _animalService.AddAnimalAsync(animal);

            var animalDto = new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name.Value,
                Species = animal.Species.Value,
                DateOfBirth = animal.DateOfBirth,
                Sex = animal.Sex.ToString(),
                FavoriteFood = animal.FavoriteFood.Value,
                Status = animal.Status.ToString(),
                CurrentEnclosureId = animal.CurrentEnclosureId
            };

            return CreatedAtAction(nameof(GetAnimalById), new { id = animal.Id }, animalDto);
        }
        catch (DomainException dex)
        {
            return BadRequest($"Validation failed: {dex.Message}");
        }
         catch (Exception ex)
         {
             return StatusCode(500, "An unexpected error occurred while creating the animal.");
         }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAnimal(Guid id)
    {
         var existing = await _animalService.GetAnimalByIdAsync(id);
         if (existing == null)
         {
             return NotFound($"Animal with ID {id} not found.");
         }

        await _animalService.DeleteAnimalAsync(id);
        return NoContent();
    }

    public record TransferAnimalRequest(Guid TargetEnclosureId);

    [HttpPost("{id:guid}/transfer")]
    [ProducesResponseType(200)] 
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> TransferAnimal(Guid id, [FromBody] TransferAnimalRequest request)
    {
        try
        {
            await _transferService.TransferAnimalAsync(id, request.TargetEnclosureId);
            return Ok($"Animal {id} successfully transferred to enclosure {request.TargetEnclosureId}.");
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
            return StatusCode(500, "An unexpected error occurred during transfer.");
        }
    }
}
