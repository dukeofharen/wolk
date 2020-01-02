using System;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class NoteBackupDto : IMapFrom<Note>
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public DateTimeOffset? Opened { get; set; }

        public long NotebookId { get; set; }
    }
}
