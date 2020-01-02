using System;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class AttachmentBackupDto : IMapFrom<Attachment>
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Filename { get; set; }

        public string MimeType { get; set; }

        public long FileSize { get; set; }

        public string InternalFilename { get; set; }

        public long NoteId { get; set;}
    }
}
