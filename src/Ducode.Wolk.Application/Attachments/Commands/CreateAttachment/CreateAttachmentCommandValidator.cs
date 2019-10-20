using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Attachments.Commands.CreateAttachment
{
    public class CreateAttachmentCommandValidator : AbstractValidator<CreateAttachmentCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public CreateAttachmentCommandValidator(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;

            RuleFor(c => c.Contents)
                .NotNull()
                .Must(c => c.Length > 0)
                .WithMessage("Please provide a file.");

            RuleFor(c => c.Filename)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(c => c.NoteId)
                .CustomAsync(NoteExistsAsync);
        }

        private async Task NoteExistsAsync(
            long noteId,
            CustomContext context,
            CancellationToken cancellationToken)
        {
            if (!await _wolkDbContext.Notes.AnyAsync(n => n.Id == noteId, cancellationToken))
            {
                context.AddFailure($"Note with ID '{noteId}' not found.");
            }
        }
    }
}
