using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Interfaces;

namespace ZooManagement.Application.Services;

public class AnimalTransferService : IAnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public AnimalTransferService(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository
        )
    {
        _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        _enclosureRepository = enclosureRepository ?? throw new ArgumentNullException(nameof(enclosureRepository));
    }

    public async Task TransferAnimalAsync(Guid animalId, Guid targetEnclosureId)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId);
        if (animal == null) throw new KeyNotFoundException($"Animal with ID {animalId} not found.");

        var targetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId);
        if (targetEnclosure == null) throw new KeyNotFoundException($"Target enclosure with ID {targetEnclosureId} not found.");

        Guid? sourceEnclosureId = animal.CurrentEnclosureId;

        if (sourceEnclosureId.HasValue && sourceEnclosureId.Value == targetEnclosureId)
        {
             Console.WriteLine($"[AppService] Animal {animal.Name} is already in enclosure {targetEnclosureId}. No transfer needed.");
            return;
        }

        Enclosure? sourceEnclosure = null;
        if (sourceEnclosureId.HasValue)
        {
            sourceEnclosure = await _enclosureRepository.GetByIdAsync(sourceEnclosureId.Value);
            if (sourceEnclosure == null)
                 Console.WriteLine($"[AppService] Warning: Source enclosure {sourceEnclosureId.Value} not found for animal {animalId}.");
        }

        targetEnclosure.AddAnimal(animal);

        sourceEnclosure?.RemoveAnimal(animal);

        await _animalRepository.UpdateAsync(animal);
        await _enclosureRepository.UpdateAsync(targetEnclosure);
        if (sourceEnclosure != null)
        {
            await _enclosureRepository.UpdateAsync(sourceEnclosure);
        }

         Console.WriteLine($"[AppService] Successfully transferred animal {animal.Name} (ID: {animalId}) from enclosure {sourceEnclosureId?.ToString() ?? "None"} to {targetEnclosureId}.");
    }
}
