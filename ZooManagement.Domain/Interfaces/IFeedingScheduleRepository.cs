using ZooManagement.Domain.Entities;

namespace ZooManagement.Domain.Interfaces;

public interface IFeedingScheduleRepository
{
    Task<FeedingSchedule?> GetByIdAsync(Guid id);
    Task<IEnumerable<FeedingSchedule>> GetAllAsync();
    Task<IEnumerable<FeedingSchedule>> GetSchedulesByAnimalIdAsync(Guid animalId);
    Task AddAsync(FeedingSchedule schedule);
    Task UpdateAsync(FeedingSchedule schedule);
    Task DeleteAsync(Guid id);
}
