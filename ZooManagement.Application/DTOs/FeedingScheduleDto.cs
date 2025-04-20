using System;

namespace ZooManagement.Application.DTOs;

public class FeedingScheduleDto
{
    public Guid Id { get; set; }
    public Guid AnimalId { get; set; }
    public DateTime FeedingTime { get; set; }
    public required string FoodType { get; set; }
    public bool IsCompleted { get; set; }
}
