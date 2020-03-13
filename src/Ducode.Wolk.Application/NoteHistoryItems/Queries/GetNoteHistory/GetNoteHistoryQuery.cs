using System.Collections.Generic;
using Ducode.Wolk.Application.NoteHistoryItems.Models;
using MediatR;

namespace Ducode.Wolk.Application.NoteHistoryItems.Queries.GetNoteHistory
{
    public class GetNoteHistoryQuery : IRequest<IEnumerable<NoteHistoryDto>>
    {
        public long NoteId { get; set; }

        public bool IncludeFullContent { get; set; }
    }
}
