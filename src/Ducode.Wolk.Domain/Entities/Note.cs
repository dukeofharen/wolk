using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public long NotebookId { get; set; }

        public Notebook Notebook { get; set; }
    }
}
