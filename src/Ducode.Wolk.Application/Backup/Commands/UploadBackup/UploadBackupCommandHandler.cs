using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Backup.Models;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Common;
using Ducode.Wolk.Common.Utilities;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ducode.Wolk.Application.Backup.Commands.UploadBackup
{
    public class UploadBackupCommandHandler : IRequestHandler<UploadBackupCommand, Unit>
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IWolkDbContext _wolkDbContext;
        private readonly WolkConfiguration _wolkConfiguration;

        public UploadBackupCommandHandler(
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

        public async Task<Unit> Handle(UploadBackupCommand request, CancellationToken cancellationToken)
        {
            IDbContextTransaction tx = null;
            try
            {
                tx = _wolkDbContext.BeginTransaction();

                // Delete access token old entries.
                var accessTokens = await _wolkDbContext.AccessTokens.ToArrayAsync(cancellationToken);
                _wolkDbContext.AccessTokens.RemoveRange(accessTokens);

                // Rename old attachments to a temporary name.
                BackupOldAttachments();

                // Delete old attachments.
                var attachments = await _wolkDbContext.Attachments.ToArrayAsync(cancellationToken);
                _wolkDbContext.Attachments.RemoveRange(attachments);

                // Delete old notes.
                var notes = await _wolkDbContext.Notes.ToArrayAsync(cancellationToken);
                _wolkDbContext.Notes.RemoveRange(notes);

                // Delete old notebooks.
                var notebooks = await _wolkDbContext.Notebooks.ToArrayAsync(cancellationToken);
                _wolkDbContext.Notebooks.RemoveRange(notebooks);

                using (var zipStream = new MemoryStream(request.ZipBytes))
                {
                    using (var zip = new ZipArchive(zipStream))
                    {
                        var notebookEntryNames = zip.ReadEntryNames(BackupConstants.NotebooksFolder);
                        foreach (var entryName in notebookEntryNames)
                        {
                            var entry = zip.ReadEntryAsText(entryName);
                            var notebook =
                                _mapper.Map<Notebook>(JsonConvert.DeserializeObject<NotebookBackupDto>(entry));
                            _wolkDbContext.Notebooks.Add(notebook);
                        }

                        var noteEntryNames = zip.ReadEntryNames(BackupConstants.NotesFolder);
                        foreach (var entryName in noteEntryNames)
                        {
                            var entry = zip.ReadEntryAsText(entryName);
                            var note = _mapper.Map<Note>(JsonConvert.DeserializeObject<NoteBackupDto>(entry));
                            _wolkDbContext.Notes.Add(note);
                        }

                        var attachmentEntryNames = zip.ReadEntryNames(BackupConstants.AttachmentsFolder);
                        foreach (var entryName in attachmentEntryNames)
                        {
                            var entry = zip.ReadEntryAsText(entryName);
                            var attachment =
                                _mapper.Map<Attachment>(JsonConvert.DeserializeObject<AttachmentBackupDto>(entry));
                            _wolkDbContext.Attachments.Add(attachment);

                            var fileEntry =
                                zip.ReadEntry($"{BackupConstants.AttachmentFilesFolder}/{attachment.Id}.bin");
                            var path = Path.Combine(_wolkConfiguration.UploadsPath, attachment.InternalFilename);
                            _fileService.WriteAllBytes(path, fileEntry);
                        }

                        var accessTokenEntryNames = zip.ReadEntryNames(BackupConstants.AccessTokensFolder);
                        foreach (var entryName in accessTokenEntryNames)
                        {
                            var entry = zip.ReadEntryAsText(entryName);
                            var accessToken = _mapper.Map<AccessToken>(JsonConvert.DeserializeObject<AccessTokenBackupDto>(entry));
                            _wolkDbContext.AccessTokens.Add(accessToken);
                        }
                    }
                }

                await _wolkDbContext.SaveChangesAsync(cancellationToken);
                tx.Commit();

                DeleteOldAttachments();
            }
            catch (Exception)
            {
                tx?.Rollback();
                RestoreOldAttachments();
                throw;
            }

            return Unit.Value;
        }

        private void BackupOldAttachments()
        {
            var attachments = _fileService.GetFiles(_wolkConfiguration.UploadsPath, string.Empty);
            foreach (var attachment in attachments)
            {
                var parts = attachment.Split('/');
                parts[^1] = parts[^1].StartsWith("_") ? parts[^1] : $"_{parts[^1]}";
                var tempFilePath = string.Join('/', parts);
                if (_fileService.FileExists(tempFilePath))
                {
                    _fileService.DeleteFile(tempFilePath);
                }

                _fileService.MoveFile(attachment, tempFilePath);
            }
        }

        private void RestoreOldAttachments()
        {
            var attachments = _fileService.GetFiles(_wolkConfiguration.UploadsPath, string.Empty);
            foreach (var attachment in attachments)
            {
                var parts = attachment.Split('/');
                parts[^1] = parts[^1].StartsWith("_") ? parts[^1].Substring(1) : parts[^1];
                _fileService.MoveFile(attachment, string.Join('/', parts));
            }
        }

        private void DeleteOldAttachments()
        {
            var attachments = _fileService
                .GetFiles(_wolkConfiguration.UploadsPath, string.Empty)
                .Where(f => Path.GetFileName(f)?.StartsWith("_") ?? false);
            foreach (var attachment in attachments)
            {
                _fileService.DeleteFile(attachment);
            }
        }
    }
}
