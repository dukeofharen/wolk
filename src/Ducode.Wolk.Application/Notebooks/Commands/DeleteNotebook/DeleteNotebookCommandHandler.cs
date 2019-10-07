using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notebooks.Commands.DeleteNotebook
{
    public class DeleteNotebookCommandHandler : IRequestHandler<DeleteNotebookCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public DeleteNotebookCommandHandler(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<Unit> Handle(DeleteNotebookCommand request, CancellationToken cancellationToken)
        {
            var notebook =
                await _wolkDbContext.Notebooks.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (notebook == null)
            {
                throw new NotFoundException(nameof(Notebook), request.Id);
            }

            _wolkDbContext.Notebooks.Remove(notebook);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
