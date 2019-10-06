using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notebooks.Commands.DeleteNotebook
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNotebookCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public DeleteNoteCommandHandler(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<Unit> Handle(DeleteNotebookCommand request, CancellationToken cancellationToken)
        {
            var note =
                await _wolkDbContext.Notes.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (note == null)
            {
                throw new NotFoundException(nameof(Notebook), request.Id);
            }

            _wolkDbContext.Notes.Remove(note);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
