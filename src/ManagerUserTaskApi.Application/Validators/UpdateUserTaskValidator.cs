namespace ManagerUserTaskApi.Application.Validators;

using Domain.Requests;
using FluentValidation;

public class UpdateUserTaskValidator : AbstractValidator<UpdateUserTask>
{
    public UpdateUserTaskValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(t => t.User)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(t => t.Date)
            .NotEmpty();

        RuleFor(t => t.StartTime)
            .NotEmpty();

        RuleFor(t => t.EndTime)
            .NotEmpty();

        RuleFor(t => t.Subject)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(t => t.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(t => t.Version)
            .GreaterThanOrEqualTo(0);
    }
}