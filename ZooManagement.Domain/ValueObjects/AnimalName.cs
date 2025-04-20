using System;
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.Domain.ValueObjects;

public record AnimalName
{
    public string Value { get; }

    public AnimalName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Animal name cannot be null or whitespace.");
        }
        Value = value;
    }
    public static implicit operator string(AnimalName name) => name.Value;
}
