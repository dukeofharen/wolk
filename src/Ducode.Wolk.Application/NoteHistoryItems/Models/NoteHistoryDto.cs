using System;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Common.Utilities;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.NoteHistoryItems.Models
{
    public class NoteHistoryDto : IHaveCustomMapping
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Preview { get; set; }

        public NoteType NoteType { get; set; }

        public DateTimeOffset OriginalCreated { get; set; }

        public DateTimeOffset? OriginalChanged { get; set; }

        public long NoteId { get; set; }

        public void CreateMappings(Profile configuration) =>
            configuration.CreateMap<NoteHistory, NoteHistoryDto>()
                .ForMember(
                    dest => dest.Preview,
                    opt => opt.MapFrom(
                        src => src.Content.Shorten(100, "...", true)));
    }
}
