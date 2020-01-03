using System;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class AttachmentBackupDto : IHaveCustomMapping
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Filename { get; set; }

        public string MimeType { get; set; }

        public long FileSize { get; set; }

        public string InternalFilename { get; set; }

        public long NoteId { get; set;}

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Attachment, AttachmentBackupDto>();
            configuration.CreateMap<AttachmentBackupDto, Attachment>()
                .ForMember(dest => dest.Note, opt => opt.Ignore());
        }
    }
}
