using ZooManagement.Domain.ValueObjects;
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.Domain.Entities;

public class Animal
{
    public Guid Id { get; private set; }
    public Species Species { get; private set; }
    public AnimalName Name { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Sex Sex { get; private set; }
    public FoodType FavoriteFood { get; private set; }
    public AnimalStatus Status { get; private set; }
    public Guid? CurrentEnclosureId { get; private set; }
    private Animal() { }

    public static Animal Create(Species species, AnimalName name, DateTime dateOfBirth, Sex sex, FoodType favoriteFood)
    {
        if (dateOfBirth > DateTime.UtcNow)
            throw new DomainException("Date of birth cannot be in the future.");

        return new Animal
        {
            Id = Guid.NewGuid(),
            Species = species,
            Name = name,
            DateOfBirth = dateOfBirth,
            Sex = sex,
            FavoriteFood = favoriteFood,
            Status = AnimalStatus.Healthy,
            CurrentEnclosureId = null
        };
    }

    public void Feed(FoodType food)
    {
        Console.WriteLine($"{Name.Value} ({Species.Value}) is being fed {food.Value}. Yum!");
    }

    public void Heal()
    {
        if (Status == AnimalStatus.Sick)
        {
            Status = AnimalStatus.Healthy;
            Console.WriteLine($"{Name.Value} has been healed.");
        }
        else
        {
             Console.WriteLine($"{Name.Value} is already healthy.");
        }
    }

    internal void AssignToEnclosure(Guid enclosureId)
    {
         if (CurrentEnclosureId.HasValue && CurrentEnclosureId.Value == enclosureId) return;
         if (Status == AnimalStatus.Sick)
            throw new DomainException($"Cannot move sick animal {Name.Value}.");

         CurrentEnclosureId = enclosureId;
         Console.WriteLine($"{Name.Value} assigned to enclosure {enclosureId}.");
    }

     internal void RemoveFromEnclosure()
     {
        CurrentEnclosureId = null;
        Console.WriteLine($"{Name.Value} removed from enclosure.");
     }
}
