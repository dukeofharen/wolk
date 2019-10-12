using FluentValidation;

namespace Ducode.Wolk.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Content)
                .NotEmpty();
        }
    }
}
