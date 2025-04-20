using System.Collections.Concurrent;
using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Interfaces;

namespace ZooManagement.Infrastructure.Persistence;

public class InMemoryAnimalRepository : IAnimalRepository
{
    private static readonly ConcurrentDictionary<Guid, Animal> _animals = new();

    public Task<Animal?> GetByIdAsync(Guid id)
    {
        _animals.TryGetValue(id, out var animal);
        return Task.FromResult(animal);
    }

    public Task<IEnumerable<Animal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Animal>>(_animals.Values.ToList());
    }

    public Task AddAsync(Animal animal)
    {
        if (animal == null) throw new ArgumentNullException(nameof(animal));

        if (!_animals.TryAdd(animal.Id, animal))
        {
            throw new InvalidOperationException($"Animal with ID {animal.Id} already exists in the repository.");
        }
        Console.WriteLine($"[Infrastructure] Animal {animal.Id} ('{animal.Name.Value}') added to In-Memory Repo.");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Animal animal)
    {
        if (animal == null) throw new ArgumentNullException(nameof(animal));

        if (!_animals.ContainsKey(animal.Id))
        {
            throw new KeyNotFoundException($"Animal with ID {animal.Id} not found for update.");
        }

        _animals[animal.Id] = animal;
        Console.WriteLine($"[Infrastructure] Animal {animal.Id} ('{animal.Name.Value}') updated in In-Memory Repo.");
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (_animals.TryRemove(id, out var removedAnimal))
        {
             Console.WriteLine($"[Infrastructure] Animal {id} ('{removedAnimal?.Name.Value}') removed from In-Memory Repo.");
        }
        else
        {
            Console.WriteLine($"[Infrastructure] Animal {id} not found for deletion in In-Memory Repo.");
        }
        return Task.CompletedTask;
    }
}
