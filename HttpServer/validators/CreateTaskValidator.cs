using FluentValidation;
using HttpServer.viewModels;

namespace HttpServer.validators;

public class CreateTaskValidator : AbstractValidator<TaskViewModel>
{
    public CreateTaskValidator()
    {
        RuleFor(employee => employee.Title)
            .MaximumLength(20)
            .WithMessage("Допустимо не более 20 символов")
            .MinimumLength(1)
            .WithMessage("Название задачи должно содержать не менее одной буквы");
        RuleFor(employee => employee.Executor)
            .MaximumLength(20)
            .WithMessage("Допустимо не более 20 символов")
            .MinimumLength(1)
            .WithMessage("Поле должно содержать не менее одной буквы")
            .Must(c => c.All(Char.IsLetter)).WithMessage("Поле не должно содержать сторонние символы");
        RuleFor(employee => employee.Description)
            .MaximumLength(100)
            .WithMessage("Не более 100 символов")
            .MinimumLength(10)
            .WithMessage("Не менее 10 символов");
    }
}