using System;

namespace ZooManagement.Domain.Exceptions;

public class EnclosureFullException : DomainException
{
    public Guid EnclosureId { get; }
    public int MaxCapacity { get; }
    public EnclosureFullException() : base("The enclosure is full.") { }
    public EnclosureFullException(string message) : base(message) { }
    public EnclosureFullException(Guid enclosureId, int maxCapacity, string message) : base(message)
    {
        EnclosureId = enclosureId;
        MaxCapacity = maxCapacity;
    }
    public EnclosureFullException(string message, Exception innerException) : base(message, innerException) { }
}
