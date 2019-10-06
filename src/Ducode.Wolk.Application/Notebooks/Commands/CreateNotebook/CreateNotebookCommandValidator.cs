using FluentValidation;

namespace Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook
{
    public class CreateNotebookCommandValidator : AbstractValidator<CreateNotebookCommand>
    {
        public CreateNotebookCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
