using ZooManagement.Domain.Entities;
using ZooManagement.Domain.Interfaces;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.Application.Services;

public class FeedingOrganizationService : IFeedingOrganizationService
{
    private readonly IFeedingScheduleRepository _scheduleRepository;
    private readonly IAnimalRepository _animalRepository;

    public FeedingOrganizationService(
        IFeedingScheduleRepository scheduleRepository,
        IAnimalRepository animalRepository
        )
    {
        _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
        _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
    }

    public async Task<Guid> AddFeedingScheduleAsync(Guid animalId, DateTime feedingTime, FoodType foodType)
    {
        // 1. Проверка существования животного
        var animal = await _animalRepository.GetByIdAsync(animalId);
        if (animal == null)
        {
            throw new KeyNotFoundException($"Cannot add schedule: Animal with ID {animalId} not found.");
        }

        var schedule = FeedingSchedule.Create(animalId, feedingTime, foodType);

        await _scheduleRepository.AddAsync(schedule);
        Console.WriteLine($"[AppService] Feeding schedule {schedule.Id} added for animal {animalId}.");

        return schedule.Id;
    }

    public async Task<FeedingSchedule?> GetScheduleByIdAsync(Guid scheduleId)
    {
        return await _scheduleRepository.GetByIdAsync(scheduleId);
    }

    public async Task<IEnumerable<FeedingSchedule>> GetAllSchedulesAsync()
    {
        return await _scheduleRepository.GetAllAsync();
    }

    public async Task<IEnumerable<FeedingSchedule>> GetSchedulesForAnimalAsync(Guid animalId)
    {
        // Опционально: проверить существование животного перед запросом?
        return await _scheduleRepository.GetSchedulesByAnimalIdAsync(animalId);
    }

    public async Task MarkScheduleAsDoneAsync(Guid scheduleId)
    {
        // 1. Получение
        var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
            throw new KeyNotFoundException($"Feeding schedule with ID {scheduleId} not found.");
        }

        schedule.MarkAsDone();

        await _scheduleRepository.UpdateAsync(schedule);
        Console.WriteLine($"[AppService] Feeding schedule {scheduleId} marked as done.");
    }

    public async Task ChangeScheduleAsync(Guid scheduleId, DateTime newTime, FoodType newFood)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
            throw new KeyNotFoundException($"Feeding schedule with ID {scheduleId} not found.");
        }

        schedule.ChangeSchedule(newTime, newFood);

        await _scheduleRepository.UpdateAsync(schedule);
        Console.WriteLine($"[AppService] Feeding schedule {scheduleId} changed.");
    }

    public async Task DeleteScheduleAsync(Guid scheduleId)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
        if (schedule == null)
        {
             Console.WriteLine($"[AppService] Feeding schedule with ID {scheduleId} not found for deletion.");
             return;
        }

        await _scheduleRepository.DeleteAsync(scheduleId);
        Console.WriteLine($"[AppService] Feeding schedule {scheduleId} deleted.");
    }
}
