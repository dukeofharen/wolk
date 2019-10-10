using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.TestUtilities.Assertions
{
    public static class NoteAssertsions
    {
        public static void ShouldBeEqual(Note note, NoteDto noteDto)
        {
            Assert.AreEqual(note.Content, noteDto.Content);
            Assert.AreEqual(note.Title, noteDto.Title);
            Assert.AreEqual(note.NotebookId, noteDto.NotebookId);
            Assert.AreEqual(note.Created, noteDto.Created);
            Assert.AreEqual(note.Changed, noteDto.Changed);
        }

        public static void ShouldBeEqual(Note note, UpdateNoteCommand command)
        {
            Assert.AreEqual(note.Content, command.Content);
            Assert.AreEqual(note.Title, command.Title);
            Assert.AreEqual(note.NotebookId, command.NotebookId);
        }
    }
}
