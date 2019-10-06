using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notes.Queries.GetNote
{
    public class GetNoteQueryHandler : IRequestHandler<GetNoteQuery, NoteDto>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public GetNoteQueryHandler(IMapper mapper, IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<NoteDto> Handle(GetNoteQuery request, CancellationToken cancellationToken)
        {
            var note = await _wolkDbContext.Notes.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (note == null)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            return _mapper.Map<NoteDto>(note);
        }
    }
}
