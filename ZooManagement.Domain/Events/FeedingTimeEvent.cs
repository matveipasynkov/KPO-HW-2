using System;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.Domain.Events;
public class FeedingTimeEvent
{
    public Guid ScheduleId { get; }
    public Guid AnimalId { get; }
    public FoodType FoodProvided { get; }
    public DateTime Timestamp { get; }

    public FeedingTimeEvent(Guid scheduleId, Guid animalId, FoodType foodProvided)
    {
         if (scheduleId == Guid.Empty)
            throw new ArgumentException("ScheduleId cannot be empty.", nameof(scheduleId));
         if (animalId == Guid.Empty)
            throw new ArgumentException("AnimalId cannot be empty.", nameof(animalId));

        ScheduleId = scheduleId;
        AnimalId = animalId;
        FoodProvided = foodProvided;
        Timestamp = DateTime.UtcNow;
    }
}

