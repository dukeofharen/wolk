using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class AttachmentFakeDataGenerator
    {
        private static readonly Faker _faker = new Faker();

        public static Attachment CreateAttachment(Note note) =>
            new Attachment
            {
                Filename = _faker.System.FileName(),
                InternalFilename = Guid.NewGuid().ToString(),
                MimeType = _faker.System.MimeType(),
                FileSize = _faker.Random.Number(),
                Note = note,
                Created = _faker.Date.Past(),
                Changed = _faker.Date.Past()
            };

        public static async Task<Attachment> CreateAndSaveAttachment(
            this IWolkDbContext wolkDbContext,
            Note note = null)
        {
            if (note == null)
            {
                note = await wolkDbContext.CreateAndSaveNote();
            }

            var attachment = CreateAttachment(note);
            wolkDbContext.Attachments.Add(attachment);
            await wolkDbContext.SaveChangesAsync(CancellationToken.None);
            return attachment;
        }
    }
}
