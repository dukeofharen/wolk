using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Notes.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notes.Queries.GetNotes
{
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, IEnumerable<NoteDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public GetNotesQueryHandler(IMapper mapper, IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<IEnumerable<NoteDto>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            var query = _wolkDbContext.Notes.AsQueryable();
            if (request.NotebookId.HasValue)
            {
                query = query.Where(n => n.NotebookId == request.NotebookId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(n => EF.Functions.Like(n.Title, $"%{request.SearchTerm}%"));
            }

            if (request.NoteType.HasValue)
            {
                query = query.Where(n => n.NoteType == request.NoteType.Value);
            }

            var result = _mapper.Map<IEnumerable<NoteDto>>(await query.ToArrayAsync(cancellationToken));
            if (!request.IncludeFullContent)
            {
                foreach (var note in result)
                {
                    note.Content = null;
                }
            }

            return result;
        }
    }
}
