using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Backup.Models;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Common;
using Ducode.Wolk.Common.Utilities;
using Ducode.Wolk.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ducode.Wolk.Application.Backup.Queries.DownloadBackup
{
    public class DownloadBackupQueryHandler : IRequestHandler<DownloadBackupQuery, byte[]>
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;
        private readonly WolkConfiguration _wolkConfiguration;

        public DownloadBackupQueryHandler(
            IFileService fileService,
            IMapper mapper,
            IWolkDbContext wolkDbContext,
            IOptions<WolkConfiguration> options)
        {
            _fileService = fileService;
            _mapper = mapper;
            _wolkDbContext = wolkDbContext;
            _wolkConfiguration = options.Value;
        }

        public async Task<byte[]> Handle(DownloadBackupQuery request, CancellationToken cancellationToken)
        {
            using (var zipStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    var notebooks =
                        _mapper.Map<IEnumerable<NotebookBackupDto>>(
                            await _wolkDbContext.Notebooks.ToArrayAsync(cancellationToken));
                    foreach (var notebook in notebooks)
                    {
                        zip.AddEntry($"{BackupConstants.NotebooksFolder}/{notebook.Id}.json",
                            JsonConvert.SerializeObject(notebook));
                    }

                    var notes = _mapper.Map<IEnumerable<NoteBackupDto>>(
                        await _wolkDbContext.Notes.ToArrayAsync(cancellationToken));
                    foreach (var note in notes)
                    {
                        zip.AddEntry($"{BackupConstants.NotesFolder}/{note.Id}.json",
                            JsonConvert.SerializeObject(note));
                    }

                    var attachments = _mapper.Map<IEnumerable<AttachmentBackupDto>>(
                        await _wolkDbContext.Attachments.ToArrayAsync(cancellationToken));
                    foreach (var attachment in attachments)
                    {
                        var path = Path.Combine(_wolkConfiguration.UploadsPath, attachment.InternalFilename);
                        if (_fileService.FileExists(path))
                        {
                            zip.AddEntry($"{BackupConstants.AttachmentsFolder}/{attachment.Id}.json",
                                JsonConvert.SerializeObject(attachment));
                            zip.AddEntry($"{BackupConstants.AttachmentFilesFolder}/{attachment.Id}.bin",
                                _fileService.ReadAllBytes(path));
                        }
                    }

                    var accessTokens = _mapper.Map<IEnumerable<AccessTokenBackupDto>>(
                        await _wolkDbContext.AccessTokens.ToArrayAsync(cancellationToken));
                    foreach (var accessToken in accessTokens)
                    {
                        zip.AddEntry($"{BackupConstants.AccessTokensFolder}/{accessToken.Id}.json",
                            JsonConvert.SerializeObject(accessToken));
                    }

                    var users = _mapper.Map<IEnumerable<UserBackupDto>>(
                        await _wolkDbContext.Users.ToArrayAsync(cancellationToken));
                    foreach (var user in users)
                    {
                        zip.AddEntry($"{BackupConstants.UsersFolder}/{user.Id}.json",
                            JsonConvert.SerializeObject(user));
                    }
                }

                return zipStream.ToArray();
            }
        }
    }
}
