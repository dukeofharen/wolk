using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Notebooks.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notebooks.Queries.GetAllNotebooks
{
    public class GetAllNotebooksQueryHandler : IRequestHandler<GetAllNotebooksQuery, IEnumerable<NotebookDto>>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public GetAllNotebooksQueryHandler(
            IMapper mapper,
            IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<IEnumerable<NotebookDto>>
            Handle(GetAllNotebooksQuery request, CancellationToken cancellationToken) =>
            _mapper.Map<IEnumerable<NotebookDto>>(await _wolkDbContext.Notebooks.ToArrayAsync(cancellationToken));
    }
}
