using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Exceptions;
using ZooManagement.Domain.Interfaces;

namespace ZooManagement.Application.Services;

public class EnclosureService : IEnclosureService
{
    private readonly IEnclosureRepository _enclosureRepository;

    public EnclosureService(IEnclosureRepository enclosureRepository)
    {
        _enclosureRepository = enclosureRepository ?? throw new ArgumentNullException(nameof(enclosureRepository));
    }

    public async Task<Enclosure?> GetEnclosureByIdAsync(Guid id)
    {
        return await _enclosureRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Enclosure>> GetAllEnclosuresAsync()
    {
        return await _enclosureRepository.GetAllAsync();
    }

    public async Task AddEnclosureAsync(Enclosure enclosure)
    {
        if (enclosure == null) throw new ArgumentNullException(nameof(enclosure));
        await _enclosureRepository.AddAsync(enclosure);
        Console.WriteLine($"[AppService] Enclosure (Type: {enclosure.Type}, ID: {enclosure.Id}) added.");
    }

    public async Task UpdateEnclosureAsync(Enclosure enclosure)
    {
        if (enclosure == null) throw new ArgumentNullException(nameof(enclosure));

        var existingEnclosure = await _enclosureRepository.GetByIdAsync(enclosure.Id);
        if (existingEnclosure == null)
        {
            throw new KeyNotFoundException($"Enclosure with ID {enclosure.Id} not found for update.");
        }

        await _enclosureRepository.UpdateAsync(enclosure);
        Console.WriteLine($"[AppService] Enclosure (ID: {enclosure.Id}) updated.");
    }

    public async Task DeleteEnclosureAsync(Guid id)
    {
        var enclosure = await _enclosureRepository.GetByIdAsync(id);
        if (enclosure == null)
        {
             Console.WriteLine($"[AppService] Enclosure with ID {id} not found for deletion.");
             return; 
        }

        if (enclosure.AnimalIds.Any())
        {
            throw new DomainException($"Cannot delete enclosure {id}: It is not empty. Contains {enclosure.CurrentAnimalCount} animal(s).");
        }

        await _enclosureRepository.DeleteAsync(id);
        Console.WriteLine($"[AppService] Enclosure with ID {id} deleted.");
    }
}
