using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Attachments.Queries.GetAttachments
{
    public class GetAttachmentsQueryHandler : IRequestHandler<GetAttachmentsQuery, IEnumerable<AttachmentDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public GetAttachmentsQueryHandler(
            IMapper mapper,
            IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<IEnumerable<AttachmentDto>> Handle(
            GetAttachmentsQuery request,
            CancellationToken cancellationToken) =>
            _mapper.Map<IEnumerable<AttachmentDto>>(await _wolkDbContext.Attachments
                .Where(a => a.NoteId == request.NoteId)
                .ToArrayAsync(cancellationToken));
    }
}
