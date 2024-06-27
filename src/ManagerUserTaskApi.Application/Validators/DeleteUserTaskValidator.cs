namespace ManagerUserTaskApi.Application.Validators;

using Domain.Requests;
using FluentValidation;

public class DeleteUserTaskValidator : AbstractValidator<DeleteUserTask>
{
    public DeleteUserTaskValidator()
    {
        RuleFor(t => t.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}