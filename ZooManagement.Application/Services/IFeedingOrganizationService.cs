using ZooManagement.Domain.Entities;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.Application.Services;

public interface IFeedingOrganizationService
{
    Task<Guid> AddFeedingScheduleAsync(Guid animalId, DateTime feedingTime, FoodType foodType);

    Task<FeedingSchedule?> GetScheduleByIdAsync(Guid scheduleId);
    Task<IEnumerable<FeedingSchedule>> GetAllSchedulesAsync();
    Task<IEnumerable<FeedingSchedule>> GetSchedulesForAnimalAsync(Guid animalId);
    Task MarkScheduleAsDoneAsync(Guid scheduleId);
    Task ChangeScheduleAsync(Guid scheduleId, DateTime newTime, FoodType newFood);
    Task DeleteScheduleAsync(Guid scheduleId);
}

