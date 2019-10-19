using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;

namespace Ducode.Wolk.Api.Models.Notebooks
{
    /// <summary>
    /// Model used for creating / updating notebooks.
    /// </summary>
    public class MutateNotebookModel : IMapTo<CreateNotebookCommand>, IMapTo<UpdateNoteCommand>
    {
        /// <summary>
        /// Gets or sets the notebook name.
        /// </summary>
        public string Name { get; set; }
    }
}
