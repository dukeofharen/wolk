using System;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Notebooks.Models
{
    public class NotebookDto : IMapFrom<Notebook>
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Changed { get; set; }

        public string Name { get; set; }
    }
}
