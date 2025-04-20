using System.Collections.Concurrent;
using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Interfaces;

namespace ZooManagement.Infrastructure.Persistence;

public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private static readonly ConcurrentDictionary<Guid, Enclosure> _enclosures = new();

    public Task<Enclosure?> GetByIdAsync(Guid id)
    {
        _enclosures.TryGetValue(id, out var enclosure);
        return Task.FromResult(enclosure);
    }

    public Task<IEnumerable<Enclosure>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Enclosure>>(_enclosures.Values.ToList());
    }

    public Task AddAsync(Enclosure enclosure)
    {
        if (enclosure == null) throw new ArgumentNullException(nameof(enclosure));

        if (!_enclosures.TryAdd(enclosure.Id, enclosure))
        {
            throw new InvalidOperationException($"Enclosure with ID {enclosure.Id} already exists in the repository.");
        }
        Console.WriteLine($"[Infrastructure] Enclosure {enclosure.Id} (Type: {enclosure.Type}) added to In-Memory Repo.");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Enclosure enclosure)
    {
        if (enclosure == null) throw new ArgumentNullException(nameof(enclosure));

        if (!_enclosures.ContainsKey(enclosure.Id))
        {
            throw new KeyNotFoundException($"Enclosure with ID {enclosure.Id} not found for update.");
        }

        _enclosures[enclosure.Id] = enclosure;
        Console.WriteLine($"[Infrastructure] Enclosure {enclosure.Id} (Type: {enclosure.Type}) updated in In-Memory Repo.");
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (_enclosures.TryRemove(id, out var removedEnclosure))
        {
            Console.WriteLine($"[Infrastructure] Enclosure {id} (Type: {removedEnclosure?.Type}) removed from In-Memory Repo.");
        }
        else
        {
             Console.WriteLine($"[Infrastructure] Enclosure {id} not found for deletion in In-Memory Repo.");
        }
        return Task.CompletedTask;
    }
}
