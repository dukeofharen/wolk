using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Backups;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.Common.Utilities;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ducode.Wolk.Api.Tests.Integration.Backup
{
    [TestClass]
    public class UploadBackup : IntegrationTestBase
    {
        [TestMethod]
        public async Task UploadBackup_ShouldRestoreEntitiesCorrectly()
        {
            // Arrange
            for (var i = 0; i < 10; i++)
            {
                await WolkDbContext.CreateAndSaveNote();
            }

            var url = "/api/backup";
            var zipPath = Path.Combine(AssemblyHelper.GetExecutingAssemblyRootPath(), "Files", "wolk-backup.zip");
            var zipBytes = File.ReadAllBytes(zipPath);
            var model = new UploadBackupModel {ZipBytes = zipBytes};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            // Assert access tokens
            var accessToken = await WolkDbContext.AccessTokens.SingleAsync();
            Assert.AreEqual("1b7d6a57-a464-47c3-9ec7-e5a30403d8c7", accessToken.Token);
            Assert.AreEqual("1", accessToken.Identifier);

            // Assert attachments
            var attachment = await WolkDbContext.Attachments.SingleAsync();
            Assert.AreEqual("b2e6ba68-4597-4404-a078-bee38e4085c8", attachment.InternalFilename);
            Assert.AreEqual(235567, attachment.FileSize);
            Assert.AreEqual("id.png", attachment.Filename);
            Assert.AreEqual(1, attachment.NoteId);

            // Assert notebooks
            var notebooks = await WolkDbContext.Notebooks.ToArrayAsync();
            Assert.AreEqual(2, notebooks.Length);

            Assert.AreEqual("Test notebook 1", notebooks[0].Name);
            Assert.AreEqual("Test notebook 2", notebooks[1].Name);

            // Assert notes
            var notes = await WolkDbContext.Notes.ToArrayAsync();
            Assert.AreEqual(4, notes.Length);

            Assert.AreEqual("Markdown test", notes[0].Title);
            Assert.AreEqual("Plain text test", notes[1].Title);
            Assert.AreEqual("Sticky notes test", notes[2].Title);
            Assert.AreEqual("Todo.txt test", notes[3].Title);

            // Assert users
            var user = await WolkDbContext.Users.SingleAsync();
            Assert.AreEqual("wolk@ducode.org", user.Email);
        }
    }
}
