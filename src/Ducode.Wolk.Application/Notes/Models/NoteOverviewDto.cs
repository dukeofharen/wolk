using System;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Common.Utilities;

namespace Ducode.Wolk.Application.Notes.Models
{
    public class NoteOverviewDto : IHaveCustomMapping
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Title { get; set; }

        public string Preview { get; set; }

        public long NotebookId { get; set; }

        public void CreateMappings(Profile configuration) =>
            configuration.CreateMap<NoteDto, NoteOverviewDto>()
                .ForMember(
                    dest => dest.Preview,
                    opt => opt.MapFrom(
                        src => src.Content.Shorten(100, "...", true)));
    }
}
