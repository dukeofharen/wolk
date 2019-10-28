using Ducode.Wolk.Api.Models.Notes;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Common.Utilities;
using Ducode.Wolk.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.TestUtilities.Assertions
{
    public static class NoteAssertions
    {
        public static void ShouldBeEqual(Note note, NoteDto noteDto)
        {
            Assert.AreEqual(note.Content, noteDto.Content);
            Assert.AreEqual(note.Title, noteDto.Title);
            Assert.AreEqual(note.NotebookId, noteDto.NotebookId);
            Assert.AreEqual(note.Created, noteDto.Created);
            Assert.AreEqual(note.Changed, noteDto.Changed);
            Assert.AreEqual(note.Opened, noteDto.Opened);
        }

        public static void ShouldBeEqual(Note note, NoteOverviewDto noteDto)
        {
            Assert.AreEqual(note.Content.Shorten(100, "...", true), noteDto.Preview);
            Assert.AreEqual(note.Title, noteDto.Title);
            Assert.AreEqual(note.NotebookId, noteDto.NotebookId);
            Assert.AreEqual(note.Created, noteDto.Created);
            Assert.AreEqual(note.Changed, noteDto.Changed);
        }

        public static void ShouldBeEqual(Note note, MutateNoteModel model)
        {
            Assert.AreEqual(note.Content, model.Content);
            Assert.AreEqual(note.Title, model.Title);
            Assert.AreEqual(note.NotebookId, model.NotebookId);
        }

        public static void ShouldBeEqual(Note note, UpdateNoteCommand command)
        {
            Assert.AreEqual(note.Content, command.Content);
            Assert.AreEqual(note.Title, command.Title);
            Assert.AreEqual(note.NotebookId, command.NotebookId);
        }
    }
}
