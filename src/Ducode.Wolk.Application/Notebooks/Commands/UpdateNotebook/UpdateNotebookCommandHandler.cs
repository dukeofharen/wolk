using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook
{
    public class UpdateNotebookCommandHandler : IRequestHandler<UpdateNotebookCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public UpdateNotebookCommandHandler(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<Unit> Handle(UpdateNotebookCommand request, CancellationToken cancellationToken)
        {
            var notebook =
                await _wolkDbContext.Notebooks.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (notebook == null)
            {
                throw new NotFoundException(nameof(Notebook), request.Id);
            }

            notebook.Name = request.Name;
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
