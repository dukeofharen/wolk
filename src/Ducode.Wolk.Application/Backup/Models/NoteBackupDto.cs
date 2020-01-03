using System;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class NoteBackupDto : IHaveCustomMapping
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public DateTimeOffset? Opened { get; set; }

        public long NotebookId { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Note, NoteBackupDto>();
            configuration.CreateMap<NoteBackupDto, Note>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.Notebook, opt => opt.Ignore());
        }
    }
}
