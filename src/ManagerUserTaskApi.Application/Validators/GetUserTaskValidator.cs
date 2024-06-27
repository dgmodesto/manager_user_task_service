namespace ManagerUserTaskApi.Application.Validators;

using Domain.Requests;
using FluentValidation;

public class GetUserTaskValidator : AbstractValidator<GetUserTask>
{
    public GetUserTaskValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}