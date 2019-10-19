using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook;

namespace Ducode.Wolk.Api.Models.Notebooks
{
    /// <summary>
    /// Model used for creating / updating notebooks.
    /// </summary>
    public class MutateNotebookModel : IHaveCustomMapping
    {
        /// <summary>
        /// Gets or sets the notebook name.
        /// </summary>
        public string Name { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MutateNotebookModel, CreateNotebookCommand>();
            configuration.CreateMap<MutateNotebookModel, UpdateNotebookCommand>();
        }
    }
}
