using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Common;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ducode.Wolk.Application.Attachments.Commands.DeleteAttachment
{
    public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand>
    {
        private readonly IFileService _fileService;
        private readonly IWolkDbContext _wolkDbContext;
        private readonly WolkConfiguration _configuration;

        public DeleteAttachmentCommandHandler(
            IFileService fileService,
            IWolkDbContext wolkDbContext,
            IOptions<WolkConfiguration> options)
        {
            _fileService = fileService;
            _wolkDbContext = wolkDbContext;
            _configuration = options.Value;
        }

        public async Task<Unit> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
        {
            var attachment = await _wolkDbContext.Attachments
                .FirstOrDefaultAsync(a => a.Id == request.AttachmentId, cancellationToken);
            if (attachment == null)
            {
                throw new NotFoundException(nameof(Attachment), request.AttachmentId);
            }

            var path = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            if (!_fileService.FileExists(path))
            {
                throw new InvalidOperationException($"File '{path}' unexpectedly not found.");
            }


            _wolkDbContext.Attachments.Remove(attachment);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            _fileService.DeleteFile(path);

            return Unit.Value;
        }
    }
}
