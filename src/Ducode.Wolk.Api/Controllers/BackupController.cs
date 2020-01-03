using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Backups;
using Ducode.Wolk.Application.Backup.Commands.UploadBackup;
using Ducode.Wolk.Application.Backup.Queries.DownloadBackup;
using Ducode.Wolk.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ducode.Wolk.Api.Controllers
{
    public class BackupController : BaseApiController
    {
        /// <summary>
        /// An endpoint for downloading all of Wolk in a .zip file.
        /// </summary>
        /// <returns>The actual .zip file.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DownloadBackup() => File(
            await Mediator.Send(
                new DownloadBackupQuery()),
            MimeTypes.Zip,
            "wolk-backup.zip");

        /// <summary>
        /// An endpoint for uploading a downloaded .zip Wolk backup file. All contents (including users) will be overwritten, so be careful!
        /// </summary>
        /// <param name="model">The model containing the uploaded .zip file.</param>
        /// <returns>Status code indicating success.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UploadBackup(UploadBackupModel model)
        {
            await Mediator.Send(Mapper.Map<UploadBackupCommand>(model));
            return NoContent();
        }
    }
}
