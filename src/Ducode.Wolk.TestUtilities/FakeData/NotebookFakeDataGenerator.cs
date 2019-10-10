using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class NotebookFakeDataGenerator
    {
        private static readonly Faker _faker = new Faker();

        public static Notebook CreateNotebook() =>
            new Notebook
            {
                Name = Guid.NewGuid().ToString(),
                Created = _faker.Date.Past(),
                Changed = _faker.Date.Past()
            };

        public static async Task<Notebook> CreateAndSaveNotebook(this IWolkDbContext wolkDbContext)
        {
            var notebook = CreateNotebook();
            wolkDbContext.Notebooks.Add(notebook);
            await wolkDbContext.SaveChangesAsync(CancellationToken.None);
            return notebook;
        }
    }
}
