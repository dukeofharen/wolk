using System;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachment;
using Ducode.Wolk.Application.Interfaces.Mappings;

namespace Ducode.Wolk.Api.Models.Attachments
{
    public class MutateAttachmentModel : IHaveCustomMapping
    {
        public string Filename { get; set; }

        public string Base64Contents { get; set; }

        public void CreateMappings(Profile configuration) =>
            configuration.CreateMap<MutateAttachmentModel, CreateAttachmentCommand>()
                .ForMember(
                    dest => dest.Contents,
                    opt => opt.MapFrom(
                        src => Convert.FromBase64String(src.Base64Contents)))
                .ForMember(dest => dest.NoteId, opt => opt.Ignore());
    }
}
