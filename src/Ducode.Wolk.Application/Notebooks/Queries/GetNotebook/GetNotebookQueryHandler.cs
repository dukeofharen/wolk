using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notebooks.Queries.GetNotebook
{
    public class GetNotebookQueryHandler : IRequestHandler<GetNotebookQuery, NotebookDto>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public GetNotebookQueryHandler(IMapper mapper, IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<NotebookDto> Handle(GetNotebookQuery request, CancellationToken cancellationToken)
        {
            var notebook =
                await _wolkDbContext.Notebooks.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (notebook == null)
            {
                throw new NotFoundException(nameof(Notebook), request.Id);
            }

            return _mapper.Map<NotebookDto>(notebook);
        }
    }
}
