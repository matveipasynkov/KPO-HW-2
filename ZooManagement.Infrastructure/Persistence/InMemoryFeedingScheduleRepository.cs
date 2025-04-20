using System.Collections.Concurrent;
using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Interfaces;

namespace ZooManagement.Infrastructure.Persistence;

public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private static readonly ConcurrentDictionary<Guid, FeedingSchedule> _schedules = new();

    public Task<FeedingSchedule?> GetByIdAsync(Guid id)
    {
        _schedules.TryGetValue(id, out var schedule);
        return Task.FromResult(schedule);
    }

    public Task<IEnumerable<FeedingSchedule>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<FeedingSchedule>>(_schedules.Values.ToList());
    }

    public Task<IEnumerable<FeedingSchedule>> GetSchedulesByAnimalIdAsync(Guid animalId)
    {
        var animalSchedules = _schedules.Values
                                        .Where(s => s.AnimalId == animalId)
                                        .ToList();
        return Task.FromResult<IEnumerable<FeedingSchedule>>(animalSchedules);
    }

    public Task AddAsync(FeedingSchedule schedule)
    {
        if (schedule == null) throw new ArgumentNullException(nameof(schedule));

        if (!_schedules.TryAdd(schedule.Id, schedule))
        {
            throw new InvalidOperationException($"Feeding schedule with ID {schedule.Id} already exists.");
        }
        Console.WriteLine($"[Infrastructure] Feeding schedule {schedule.Id} for animal {schedule.AnimalId} added to In-Memory Repo.");
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FeedingSchedule schedule)
    {
        if (schedule == null) throw new ArgumentNullException(nameof(schedule));

        if (!_schedules.ContainsKey(schedule.Id))
        {
            throw new KeyNotFoundException($"Feeding schedule with ID {schedule.Id} not found for update.");
        }

        _schedules[schedule.Id] = schedule;
        Console.WriteLine($"[Infrastructure] Feeding schedule {schedule.Id} for animal {schedule.AnimalId} updated in In-Memory Repo.");
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (_schedules.TryRemove(id, out var removedSchedule))
        {
             Console.WriteLine($"[Infrastructure] Feeding schedule {id} for animal {removedSchedule?.AnimalId} removed from In-Memory Repo.");
        }
        else
        {
            Console.WriteLine($"[Infrastructure] Feeding schedule {id} not found for deletion in In-Memory Repo.");
        }
        return Task.CompletedTask;
    }
}
