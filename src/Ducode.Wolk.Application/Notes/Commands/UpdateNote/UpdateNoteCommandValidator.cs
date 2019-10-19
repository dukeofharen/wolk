using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities.Enums;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public UpdateNoteCommandValidator(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;

            RuleFor(c => c.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Content)
                .NotEmpty();

            RuleFor(c => c.NoteType)
                .IsInEnum()
                .NotEqual(NoteType.NotSet);

            RuleFor(c => c.NotebookId)
                .CustomAsync(NotebookExistsAsync);
        }

        private async Task NotebookExistsAsync(
            long notebookId,
            CustomContext context,
            CancellationToken cancellationToken)
        {
            if (!await _wolkDbContext.Notebooks.AnyAsync(n => n.Id == notebookId, cancellationToken))
            {
                context.AddFailure($"Notebook with ID '{notebookId}' not found.");
            }
        }
    }
}
