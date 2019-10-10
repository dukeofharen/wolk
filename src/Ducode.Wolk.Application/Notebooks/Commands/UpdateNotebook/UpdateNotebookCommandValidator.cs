using FluentValidation;

namespace Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook
{
    public class UpdateNotebookCommandValidator : AbstractValidator<UpdateNotebookCommand>
    {
        public UpdateNotebookCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
