using System;
using System.Collections.Generic;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public DateTimeOffset? Opened { get; set; }

        public long NotebookId { get; set; }

        public Notebook Notebook { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public ICollection<NoteHistory> NoteHistory { get; set; }
    }
}
