using System;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Notes.Models
{
    public class NoteDto : IMapFrom<Note>
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Changed { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long NotebookId { get; set; }
    }
}
