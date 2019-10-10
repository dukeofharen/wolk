using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class NoteFakeDataGenerator
    {
        private static readonly Faker _faker = new Faker();

        public static Note CreateNote(Notebook notebook) =>
            new Note
            {
                Title = Guid.NewGuid().ToString(),
                Content = _faker.Lorem.Paragraphs(10),
                Created = _faker.Date.Past(),
                Changed = _faker.Date.Past(),
                Notebook = notebook
            };

        public static async Task<Note> CreateAndSaveNote(this IWolkDbContext wolkDbContext, Notebook notebook = null)
        {
            if (notebook == null)
            {
                notebook = await wolkDbContext.CreateAndSaveNotebook();
            }

            var note = CreateNote(notebook);
            wolkDbContext.Notes.Add(note);
            await wolkDbContext.SaveChangesAsync(CancellationToken.None);
            return note;
        }
    }
}
