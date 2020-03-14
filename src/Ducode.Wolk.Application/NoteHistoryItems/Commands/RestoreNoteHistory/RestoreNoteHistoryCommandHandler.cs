using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using MediatR;

namespace Ducode.Wolk.Application.NoteHistoryItems.Commands.RestoreNoteHistory
{
    public class RestoreNoteHistoryCommandHandler : IRequestHandler<RestoreNoteHistoryCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public RestoreNoteHistoryCommandHandler(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<Unit> Handle(RestoreNoteHistoryCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
