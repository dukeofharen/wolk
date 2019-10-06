namespace Ducode.Wolk.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public long NotebookId { get; set; }

        public Notebook Notebook { get; set; }
    }
}
