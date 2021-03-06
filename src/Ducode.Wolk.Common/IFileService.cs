using System;

namespace Ducode.Wolk.Common
{
    public interface IFileService
    {
        byte[] ReadAllBytes(string path);

        string ReadAllText(string path);

        void WriteAllBytes(string path, byte[] contents);

        void WriteAllText(string path, string contents);

        bool FileExists(string path);

        bool DirectoryExists(string path);

        void CreateDirectory(string path);

        string GetTempPath();

        void DeleteFile(string path);

        DateTime GetLastWriteTime(string path);

        bool IsDirectory(string path);

        string[] GetFiles(string path, string searchPattern);

        string GetCurrentDirectory();

        DateTime GetModicationDateTime(string path);

        void MoveFile(string oldPath, string newPath);
    }
}
