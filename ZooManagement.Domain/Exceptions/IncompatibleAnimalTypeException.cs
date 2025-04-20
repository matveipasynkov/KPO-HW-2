using System;
using ZooManagement.Domain.ValueObjects;

namespace ZooManagement.Domain.Exceptions;
public class IncompatibleAnimalTypeException : DomainException
{
    public Guid EnclosureId { get; }
    public EnclosureType EnclosureType { get; }
    public Species AnimalSpecies { get; }
    public IncompatibleAnimalTypeException() : base("The animal type is incompatible with the enclosure type.") { }
    public IncompatibleAnimalTypeException(string message) : base(message) { }
    public IncompatibleAnimalTypeException(Guid enclosureId, EnclosureType enclosureType, Species animalSpecies, string message) : base(message)
    {
        EnclosureId = enclosureId;
        EnclosureType = enclosureType;
        AnimalSpecies = animalSpecies;
    }
    public IncompatibleAnimalTypeException(string message, Exception innerException) : base(message, innerException) { }
}

