using ZooManagement.Domain.ValueObjects;
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.Domain.Entities;

public class Enclosure
{
    public Guid Id { get; private set; }
    public EnclosureType Type { get; private set; }
    public int Size { get; private set; }
    public int MaxCapacity { get; private set; }
    public List<Guid> AnimalIds { get; private set; } = new List<Guid>();

    public int CurrentAnimalCount => AnimalIds.Count;
    public bool IsFull => CurrentAnimalCount >= MaxCapacity;

    private Enclosure() { }

    public static Enclosure Create(EnclosureType type, int size, int maxCapacity)
    {
         if (size <= 0 || maxCapacity <= 0)
            throw new DomainException("Size and capacity must be positive.");

        return new Enclosure
        {
            Id = Guid.NewGuid(),
            Type = type,
            Size = size,
            MaxCapacity = maxCapacity
        };
    }

    public void AddAnimal(Animal animal)
    {
        if (IsFull)
            throw new EnclosureFullException($"Enclosure {Id} ({Type}) is full.");

        if (!CanAccommodate(animal.Species))
             throw new IncompatibleAnimalTypeException($"Enclosure type {Type} cannot accommodate species {animal.Species.Value}.");

        if (AnimalIds.Contains(animal.Id)) return;

        AnimalIds.Add(animal.Id);
        animal.AssignToEnclosure(this.Id);
         Console.WriteLine($"Animal {animal.Name.Value} added to enclosure {Id}. Current count: {CurrentAnimalCount}");
    }

    public void RemoveAnimal(Animal animal)
    {
        if (AnimalIds.Remove(animal.Id))
        {
             animal.RemoveFromEnclosure();
             Console.WriteLine($"Animal {animal.Name.Value} removed from enclosure {Id}. Current count: {CurrentAnimalCount}");
        }
    }

    public bool CanAccommodate(Species species)
    {
        switch (Type)
        {
            case EnclosureType.Predator:
                return species.Value.Contains("Lion") || species.Value.Contains("Tiger");
            case EnclosureType.Herbivore:
                 return species.Value.Contains("Zebra") || species.Value.Contains("Giraffe");
            case EnclosureType.Avian:
                 return species.Value.Contains("Eagle") || species.Value.Contains("Parrot");
             case EnclosureType.Aquarium:
                 return species.Value.Contains("Fish") || species.Value.Contains("Shark");
            default:
                return false;
        }
    }

    public void Clean()
    {
         Console.WriteLine($"Enclosure {Id} ({Type}) is being cleaned.");
    }
}

