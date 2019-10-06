using Ducode.Wolk.Application.Notes.Models;
using MediatR;

namespace Ducode.Wolk.Application.Notes.Queries.GetNote
{
    public class GetNoteQuery : IRequest<NoteDto>
    {
        public GetNoteQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
