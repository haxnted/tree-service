using CSharpFunctionalExtensions;
using TreeService.Domain.Shared;

namespace TreeService.Domain.ValueObjects;

/// <summary>
/// Заголовок
/// </summary>
public class Title : ValueObject
{
    public const int MAX_TITLE_LENGHT = 100;
    public string Value { get; private set; }

    private Title() { }
    private Title(string value)
    {
        Value = value;
    }
    
    public static Result<Title, Error> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Errors.Node.ValueIsRequired("title");

        if (title.Length > MAX_TITLE_LENGHT)
            return Errors.Node.ValueIsRequired($"title cannot be longer than {MAX_TITLE_LENGHT} characters");
        
        return new Title(title);
    }
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}
