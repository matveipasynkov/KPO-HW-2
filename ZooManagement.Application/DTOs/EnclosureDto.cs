using System;
using System.Collections.Generic;

namespace ZooManagement.Application.DTOs;

public class EnclosureDto
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public int Size { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentAnimalCount { get; set; }
    public List<Guid> AnimalIds { get; set; } = new List<Guid>();
}
