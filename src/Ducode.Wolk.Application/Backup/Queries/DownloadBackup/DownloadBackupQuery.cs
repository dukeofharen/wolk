using MediatR;

namespace Ducode.Wolk.Application.Backup.Queries.DownloadBackup
{
    public class DownloadBackupQuery : IRequest<byte[]>
    {
    }
}
