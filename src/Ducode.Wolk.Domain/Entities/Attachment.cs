namespace Ducode.Wolk.Domain.Entities
{
    public class Attachment : BaseEntity
    {
        public string Filename { get; set; }

        public string MimeType { get; set; }

        public long NoteId { get; set;}

        public Note Note { get; set; }
    }
}
