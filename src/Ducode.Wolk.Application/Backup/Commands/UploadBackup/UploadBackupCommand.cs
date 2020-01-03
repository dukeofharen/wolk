using MediatR;

namespace Ducode.Wolk.Application.Backup.Commands.UploadBackup
{
    public class UploadBackupCommand : IRequest
    {
        public byte[] ZipBytes { get; set; }
    }
}
