using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Common;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ducode.Wolk.Application.Attachments.Commands.CreateAttachment
{
    public class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, AttachmentDto>
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IMimeService _mimeService;
        private readonly IWolkDbContext _wolkDbContext;
        private readonly WolkConfiguration _wolkConfiguration;

        public CreateAttachmentCommandHandler(
            IFileService fileService,
            IMapper mapper,
            IMimeService mimeService,
            IWolkDbContext wolkDbContext,
            IOptions<WolkConfiguration> options)
        {
            _fileService = fileService;
            _mapper = mapper;
            _mimeService = mimeService;
            _wolkDbContext = wolkDbContext;
            _wolkConfiguration = options.Value;
        }

        public async Task<AttachmentDto> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var internalFilename = Guid.NewGuid().ToString();
            var attachment = new Attachment
            {
                Filename = request.Filename,
                NoteId = request.NoteId,
                FileSize = request.Contents.Length,
                InternalFilename = internalFilename,
                MimeType = _mimeService.GetMimeType(request.Filename)
            };
            _wolkDbContext.Attachments.Add(attachment);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);

            var attachmentPath = Path.Combine(_wolkConfiguration.UploadsPath, internalFilename);
            _fileService.WriteAllBytes(attachmentPath, request.Contents);

            return _mapper.Map<AttachmentDto>(attachment);
        }
    }
}
