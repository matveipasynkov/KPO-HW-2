using ZooManagement.Application.DTOs;
using ZooManagement.Domain.Interfaces;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.Application.Services;

public class ZooStatisticsService : IZooStatisticsService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public ZooStatisticsService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
    {
        _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        _enclosureRepository = enclosureRepository ?? throw new ArgumentNullException(nameof(enclosureRepository));
    }

    public async Task<ZooStatisticsDto> GetStatisticsAsync()
    {
        var animals = (await _animalRepository.GetAllAsync()).ToList();
        var enclosures = (await _enclosureRepository.GetAllAsync()).ToList();

        int totalAnimals = animals.Count;
        int totalEnclosures = enclosures.Count;
        int enclosuresWithCapacity = enclosures.Count(e => !e.IsFull);

        var animalsByEnclosureType = enclosures
            .GroupBy(e => e.Type)
            .ToDictionary(
                g => g.Key.ToString(),
                g => g.Sum(e => e.CurrentAnimalCount)
            );

        foreach (EnclosureType type in Enum.GetValues(typeof(EnclosureType)))
        {
            if (!animalsByEnclosureType.ContainsKey(type.ToString()))
            {
                animalsByEnclosureType.Add(type.ToString(), 0);
            }
        }

        var statsDto = new ZooStatisticsDto
        {
            TotalAnimals = totalAnimals,
            TotalEnclosures = totalEnclosures,
            EnclosuresWithCapacity = enclosuresWithCapacity,
            AnimalsByEnclosureType = animalsByEnclosureType
        };

         Console.WriteLine($"[AppService] Statistics calculated: Animals={totalAnimals}, Enclosures={totalEnclosures}");
        return statsDto;
    }
}
