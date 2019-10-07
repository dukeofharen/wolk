using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class NotebookFakeDataGenerator
    {
        private static Random _random = new Random();

        public static Notebook CreateNotebook() =>
            new Notebook
            {
                Name = Guid.NewGuid().ToString(),
                Created = DateTime.Now.AddDays(-_random.Next(0, 100)),
                Changed = DateTime.Now.AddDays(-_random.Next(0, 100))
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
