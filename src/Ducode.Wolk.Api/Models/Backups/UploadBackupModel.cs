using Ducode.Wolk.Application.Backup.Commands.UploadBackup;
using Ducode.Wolk.Application.Interfaces.Mappings;

namespace Ducode.Wolk.Api.Models.Backups
{
    public class UploadBackupModel : IMapTo<UploadBackupCommand>
    {
        public byte[] ZipBytes { get; set; }
    }
}
