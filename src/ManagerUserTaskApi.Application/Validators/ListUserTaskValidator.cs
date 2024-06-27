namespace ManagerUserTaskApi.Application.Validators;

using Domain.Requests;
using FluentValidation;

public class ListUserTaskValidator : AbstractValidator<ListTasks>
{
    public ListUserTaskValidator()
    {
        RuleFor(l => l.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(l => l.PageSize)
            .InclusiveBetween(1, 100);
    }
}