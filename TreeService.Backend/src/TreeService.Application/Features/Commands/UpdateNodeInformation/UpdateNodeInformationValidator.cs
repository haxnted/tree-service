using FluentValidation;
using TreeService.Application.Validator;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Features.Commands.UpdateNodeInformation;

public class UpdateNodeInformationValidator : AbstractValidator<UpdateNodeInformationCommand>
{
    public UpdateNodeInformationValidator()
    {
        RuleFor(c => c.Title)
            .MustBeValueObject(Title.Create);

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
    }
}
