using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Attachments.Models
{
    public class FullAttachmentDto : AttachmentDto, IHaveCustomMapping
    {
        public byte[] Contents { get; set; }

        public void CreateMappings(Profile configuration) => configuration
            .CreateMap<Attachment, FullAttachmentDto>()
            .ForMember(dest => dest.Contents, opt => opt.Ignore());
    }
}
