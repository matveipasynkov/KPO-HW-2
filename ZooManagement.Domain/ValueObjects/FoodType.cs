using System;
using ZooManagement.Domain.Exceptions;

namespace ZooManagement.Domain.ValueObjects;
public record FoodType
{
    public string Value { get; }

    public FoodType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Food type cannot be null or whitespace.");
        }
        Value = value;
    }

    public static implicit operator string(FoodType foodType) => foodType.Value;
}
