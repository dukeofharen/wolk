using AutoMapper;
using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Api.Models.Notes
{
    /// <summary>
    /// Model used for creating / updating notes.
    /// </summary>
    public class MutateNoteModel : IHaveCustomMapping
    {
        /// <summary>
        /// Gets or sets the note title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the note content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the note type.
        /// </summary>
        public NoteType NoteType { get; set; }

        /// <summary>
        /// Gets or sets the notebook ID.
        /// </summary>
        public long NotebookId { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MutateNoteModel, CreateNoteCommand>();
            configuration.CreateMap<MutateNoteModel, UpdateNoteCommand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
