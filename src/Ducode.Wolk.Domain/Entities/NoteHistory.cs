using System;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Domain.Entities
{
    public class NoteHistory : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public DateTimeOffset OriginalCreated { get; set; }

        public DateTimeOffset? OriginalChanged { get; set; }

        public long NoteId { get; set; }

        public Note Note { get; set; }
    }
}
