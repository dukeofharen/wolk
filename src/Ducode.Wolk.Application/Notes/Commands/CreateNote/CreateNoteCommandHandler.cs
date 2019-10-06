using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDto>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public CreateNoteCommandHandler(IMapper mapper, IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            if (!await _wolkDbContext.Notebooks.AnyAsync(n => n.Id == request.NotebookId, cancellationToken))
            {
                throw new NotFoundException(nameof(Notebook), request.NotebookId);
            }

            var note = new Note
            {
                Title = request.Title,
                Content = request.Content,
                NotebookId = request.NotebookId
            };
            _wolkDbContext.Notes.Add(note);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<NoteDto>(note);
        }
    }
}
