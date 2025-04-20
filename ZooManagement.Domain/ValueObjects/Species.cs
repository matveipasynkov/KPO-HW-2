using System;
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.Domain.ValueObjects;

public record Species
{
    public string Value { get; }

    public Species(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Species cannot be null or whitespace.");
        }
        Value = value;
    }

    public static implicit operator string(Species species) => species.Value;
}
