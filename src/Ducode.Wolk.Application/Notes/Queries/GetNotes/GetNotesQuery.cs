using System.Collections.Generic;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Domain.Entities.Enums;
using MediatR;

namespace Ducode.Wolk.Application.Notes.Queries.GetNotes
{
    public class GetNotesQuery : IRequest<IEnumerable<NoteDto>>
    {
        public long? NotebookId { get; set; }

        public string SearchTerm { get; set; }

        public NoteType? NoteType { get; set; }

        public bool IncludeFullContent { get; set; }
    }
}
