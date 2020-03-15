using MediatR;

namespace Ducode.Wolk.Application.NoteHistoryItems.Notifications.SaveNoteHistory
{
    public class SaveNoteHistoryNotification : INotification
    {
        public long NoteId { get; set; }
    }
}
