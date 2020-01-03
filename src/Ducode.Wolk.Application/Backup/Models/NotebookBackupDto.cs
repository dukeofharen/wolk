using System;
using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Backup.Models
{
    public class NotebookBackupDto : IHaveCustomMapping
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Changed { get; set; }

        public string Name { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Notebook, NotebookBackupDto>();
            configuration.CreateMap<NotebookBackupDto, Notebook>()
                .ForMember(dest => dest.Notes, opt => opt.Ignore());
        }
    }
}
