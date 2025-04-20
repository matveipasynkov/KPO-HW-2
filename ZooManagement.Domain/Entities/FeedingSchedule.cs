using ZooManagement.Domain.ValueObjects;
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.Domain.Entities;

public class FeedingSchedule
{
    public Guid Id { get; private set; }
    public Guid AnimalId { get; private set; }
    public DateTime FeedingTime { get; private set; }
    public FoodType FoodType { get; private set; }
    public bool IsCompleted { get; private set; }

    private FeedingSchedule() { }

    public static FeedingSchedule Create(Guid animalId, DateTime feedingTime, FoodType foodType)
    {
        if (animalId == Guid.Empty)
            throw new DomainException("AnimalId cannot be empty for a feeding schedule.");
        if (feedingTime <= DateTime.UtcNow)
            throw new DomainException("Feeding time must be in the future.");

        return new FeedingSchedule
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            FeedingTime = feedingTime,
            FoodType = foodType,
            IsCompleted = false
        };
    }

    public void ChangeSchedule(DateTime newFeedingTime, FoodType newFoodType)
    {
        if (IsCompleted)
            throw new DomainException("Cannot change schedule for a completed feeding.");
        if (newFeedingTime <= DateTime.UtcNow)
            throw new DomainException("New feeding time must be in the future.");

        FeedingTime = newFeedingTime;
        FoodType = newFoodType;
         Console.WriteLine($"Schedule {Id} for animal {AnimalId} changed to {FeedingTime} with food {FoodType.Value}.");
    }

    public void MarkAsDone()
    {
        if (IsCompleted)
        {
             Console.WriteLine($"Feeding schedule {Id} for animal {AnimalId} is already marked as done.");
             return;
        }

        IsCompleted = true;
        Console.WriteLine($"Feeding schedule {Id} for animal {AnimalId} marked as completed at {DateTime.UtcNow}.");
    }
}
