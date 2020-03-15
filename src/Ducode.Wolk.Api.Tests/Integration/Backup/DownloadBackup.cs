using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Backups;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.Common.Utilities;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ducode.Wolk.Api.Tests.Integration.Backup
{
    [TestClass]
    public class DownloadBackup : IntegrationTestBase
    {
        [TestMethod]
        public async Task UploadBackup_ThenDownloadBackup_ShouldWorkCorrectly()
        {
            // Arrange
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

            // Upload
            using var response = await HttpClient.SendAsync(request);

            // Assert upload
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            // Download
            request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };
            token = await GetJwt(await WolkDbContext.Users.SingleAsync());
            request.AddJwtBearer(token);

            using var downloadResponse = await HttpClient.SendAsync(request);

            // Assert download
            Assert.AreEqual(HttpStatusCode.OK, downloadResponse.StatusCode);

            var content = await downloadResponse.Content.ReadAsByteArrayAsync();
            Assert.AreEqual(240115, content.Length);
        }
    }
}
