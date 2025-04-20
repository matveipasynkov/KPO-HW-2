using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Interfaces;

namespace ZooManagement.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public AnimalService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
    {
        _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        _enclosureRepository = enclosureRepository ?? throw new ArgumentNullException(nameof(enclosureRepository));
    }

    public async Task<Animal?> GetAnimalByIdAsync(Guid id)
    {
        return await _animalRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
    {
        return await _animalRepository.GetAllAsync();
    }

    public async Task AddAnimalAsync(Animal animal)
    {
        if (animal == null) throw new ArgumentNullException(nameof(animal));
        await _animalRepository.AddAsync(animal);
        Console.WriteLine($"[AppService] Animal '{animal.Name}' added.");
    }

    public async Task UpdateAnimalAsync(Animal animal)
    {
        if (animal == null) throw new ArgumentNullException(nameof(animal));

        var existingAnimal = await _animalRepository.GetByIdAsync(animal.Id);
        if (existingAnimal == null)
        {
            throw new KeyNotFoundException($"Animal with ID {animal.Id} not found for update.");
        }

        await _animalRepository.UpdateAsync(animal);
        Console.WriteLine($"[AppService] Animal '{animal.Name}' (ID: {animal.Id}) updated.");
    }

    public async Task DeleteAnimalAsync(Guid id)
    {
        var animal = await _animalRepository.GetByIdAsync(id);
        if (animal == null)
        {
            Console.WriteLine($"[AppService] Animal with ID {id} not found for deletion.");
            return;
        }

        if (animal.CurrentEnclosureId.HasValue)
        {
            var enclosure = await _enclosureRepository.GetByIdAsync(animal.CurrentEnclosureId.Value);
            if (enclosure != null)
            {
                try
                {
                    enclosure.RemoveAnimal(animal);
                    await _enclosureRepository.UpdateAsync(enclosure);
                    Console.WriteLine($"[AppService] Animal '{animal.Name}' removed from enclosure {enclosure.Id}.");
                }
                catch (Exception ex)
                {
                     Console.WriteLine($"[AppService] Warning: Failed to remove animal {id} from enclosure {enclosure.Id} before deletion: {ex.Message}");
                }
            }
            else
            {
                 Console.WriteLine($"[AppService] Warning: Enclosure {animal.CurrentEnclosureId.Value} not found for animal {id} during deletion.");
            }
        }

        await _animalRepository.DeleteAsync(id);
        Console.WriteLine($"[AppService] Animal with ID {id} deleted.");
    }
}
