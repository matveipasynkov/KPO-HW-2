using System.Collections.Generic;

namespace ZooManagement.Application.DTOs;

public class ZooStatisticsDto
{
    public int TotalAnimals { get; set; }
    public int TotalEnclosures { get; set; }
    public int EnclosuresWithCapacity { get; set; }
    public Dictionary<string, int> AnimalsByEnclosureType { get; set; } = new Dictionary<string, int>();
}
