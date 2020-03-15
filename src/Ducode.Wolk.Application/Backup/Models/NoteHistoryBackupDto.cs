using System;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class NoteHistoryBackupDto : IHaveCustomMapping
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public DateTimeOffset OriginalCreated { get; set; }

        public DateTimeOffset? OriginalChanged { get; set; }

        public long NoteId { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<NoteHistory, NoteHistoryBackupDto>();
            configuration.CreateMap<NoteHistoryBackupDto, NoteHistory>()
                .ForMember(dest => dest.Note, opt => opt.Ignore());
        }
    }
}
