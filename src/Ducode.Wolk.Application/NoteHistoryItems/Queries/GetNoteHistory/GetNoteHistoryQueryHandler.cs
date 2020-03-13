using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.NoteHistoryItems.Models;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.NoteHistoryItems.Queries.GetNoteHistory
{
    public class GetNoteHistoryQueryHandler : IRequestHandler<GetNoteHistoryQuery, IEnumerable<NoteHistoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public GetNoteHistoryQueryHandler(IWolkDbContext wolkDbContext, IMapper mapper)
        {
            _wolkDbContext = wolkDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NoteHistoryDto>> Handle(
            GetNoteHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var note = await _wolkDbContext.Notes
                .Include(n => n.NoteHistory)
                .FirstOrDefaultAsync(n => n.Id == request.NoteId, cancellationToken);
            if (note == null)
            {
                throw new NotFoundException(nameof(Note), request.NoteId);
            }

            var result = _mapper.Map<IEnumerable<NoteHistoryDto>>(note.NoteHistory);
            if (!request.IncludeFullContent)
            {
                foreach (var item in result)
                {
                    item.Content = null;
                }
            }

            return result;
        }
    }
}
