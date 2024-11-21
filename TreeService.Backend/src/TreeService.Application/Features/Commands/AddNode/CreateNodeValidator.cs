using FluentValidation;
using TreeService.Application.Validator;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Features.Commands.AddNode;

public class CreateNodeValidator : AbstractValidator<CreateNodeCommand>
{
    public CreateNodeValidator()
    {
        RuleFor(c => c.Title)
            .MustBeValueObject(Title.Create);

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
    }
}
