using System;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachmentAccessToken;
using Ducode.Wolk.Application.Interfaces.Mappings;

namespace Ducode.Wolk.Api.Models.Attachments
{
    public class MutateAttachmentAccessTokenModel : IHaveCustomMapping
    {
        public DateTimeOffset? ExpirationDateTime { get; set; }

        public void CreateMappings(Profile configuration) =>
            configuration.CreateMap<MutateAttachmentAccessTokenModel, CreateAttachmentAccessTokenCommand>()
                .ForMember(dest => dest.AttachmentId, opt => opt.Ignore());
    }
}
