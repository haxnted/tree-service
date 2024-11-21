using CSharpFunctionalExtensions;
using TreeService.Domain.Shared;

namespace TreeService.Domain.ValueObjects;

/// <summary>
/// Описание
/// </summary>
public class Description : ValueObject
{
    public const int MAX_DESCRIPTION_LENGHT = 400;
    public string Value { get; private set; }

    private Description() { }
    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Errors.Node.ValueIsRequired("description");

        if (title.Length > MAX_DESCRIPTION_LENGHT)
            return Errors.Node.ValueIsRequired(
                $"description cannot be longer than {MAX_DESCRIPTION_LENGHT} characters");

        return new Description(title);
    }


    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
