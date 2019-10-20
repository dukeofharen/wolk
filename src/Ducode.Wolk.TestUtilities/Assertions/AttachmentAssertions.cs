using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.TestUtilities.Assertions
{
    public static class AttachmentAssertions
    {
        public static void ShouldBeEqual(Attachment attachment, AttachmentDto attachmentDto)
        {
            Assert.AreEqual(attachment.Filename, attachmentDto.Filename);
            Assert.AreEqual(attachment.MimeType, attachmentDto.MimeType);
            Assert.AreEqual(attachment.FileSize, attachmentDto.FileSize);
            Assert.AreEqual(attachment.NoteId, attachmentDto.NoteId);
            Assert.AreEqual(attachment.Created, attachmentDto.Created);
            Assert.AreEqual(attachment.Changed, attachmentDto.Changed);
        }
    }
}
