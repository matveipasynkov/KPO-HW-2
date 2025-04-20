using System;

namespace ZooManagement.Application.DTOs;

public class AnimalDto
{
    public Guid Id { get; set; }
    public required string Species { get; set; } 
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Sex { get; set; }
    public required string FavoriteFood { get; set; }
    public required string Status { get; set; }
    public Guid? CurrentEnclosureId { get; set; }
}
