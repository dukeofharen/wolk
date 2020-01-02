using System.Threading.Tasks;
using Ducode.Wolk.Application.Backup.Queries.DownloadBackup;
using Ducode.Wolk.Common.Constants;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DownloadBackup() => File(
            await Mediator.Send(
                new DownloadBackupQuery()),
            MimeTypes.Zip,
            "wolk-backup.zip");
    }
}
