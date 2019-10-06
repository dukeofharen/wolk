using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Domain.Entities;
using MediatR;

namespace Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook
{
    public class CreateNotebookCommandHandler : IRequestHandler<CreateNotebookCommand, NotebookDto>
    {
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;

        public CreateNotebookCommandHandler(IMapper mapper, IWolkDbContext wolkDbContext)
        {
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<NotebookDto> Handle(CreateNotebookCommand request, CancellationToken cancellationToken)
        {
            var notebook = new Notebook {Name = request.Name};
            _wolkDbContext.Notebooks.Add(notebook);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<NotebookDto>(notebook);
        }
    }
}
