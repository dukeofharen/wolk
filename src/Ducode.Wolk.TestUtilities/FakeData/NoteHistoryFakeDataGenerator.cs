using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class NoteHistoryFakeDataGenerator
    {
        private static readonly Faker _faker = new Faker();

        public static NoteHistory CreateNoteHistory(Note note) =>
            new NoteHistory
            {
                Changed = _faker.Date.Past(),
                Content = _faker.Lorem.Paragraphs(10),
                Created = _faker.Date.Past(),
                Note = note,
                Title = Guid.NewGuid().ToString(),
                NoteType = _faker.PickRandom<NoteType>(),
                OriginalChanged = _faker.Date.Past(),
                OriginalCreated = _faker.Date.Past()
            };

        public static async Task<NoteHistory> CreateAndSaveNoteHistory(this IWolkDbContext wolkDbContext, Note note)
        {
            var history = CreateNoteHistory(note);
            wolkDbContext.NoteHistory.Add(history);
            await wolkDbContext.SaveChangesAsync(CancellationToken.None);
            return history;
        }
    }
}
