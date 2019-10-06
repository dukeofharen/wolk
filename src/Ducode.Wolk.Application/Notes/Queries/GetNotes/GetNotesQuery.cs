using System.Collections.Generic;
using Ducode.Wolk.Application.Notes.Models;
using MediatR;

namespace Ducode.Wolk.Application.Notes.Queries.GetNotes
{
    public class GetNotesQuery : IRequest<IEnumerable<NoteDto>>
    {
        public long? NotebookId { get; set; }
    }
}
