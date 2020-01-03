using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ducode.Wolk.Common;

namespace Ducode.Wolk.TestUtilities.Files
{
    public class MockFileService : IFileService
    {
        public class MockFileEntry
        {
            private MockFileEntry() { }

            public MockFileEntry(string path, byte[] contents)
            {
                Path = path;
                Contents = contents;
            }

            public MockFileEntry(string path, string contents)
            {
                Path = path;
                Contents = Encoding.UTF8.GetBytes(contents);
            }

            public string Path { get; set; }

            public byte[] Contents { get; set; }
        }

        private readonly IList<MockFileEntry> _files = new List<MockFileEntry>();

        public IList<MockFileEntry> Files => _files;

        public byte[] ReadAllBytes(string path) => _files.FirstOrDefault(f => f.Path == path)?.Contents;

        public string ReadAllText(string path)
        {
            var contents = _files.FirstOrDefault(f => f.Path == path)?.Contents;
            return contents == null ? null : Encoding.UTF8.GetString(contents);
        }

        public void WriteAllBytes(string path, byte[] contents) => _files.Add(new MockFileEntry(path, contents));

        public void WriteAllText(string path, string contents) => _files.Add(new MockFileEntry(path, contents));

        public bool FileExists(string path) => _files.Any(f => f.Path == path);

        public bool DirectoryExists(string path) => _files.Any(f => f.Path.StartsWith(path));

        public void CreateDirectory(string path)
        {
            // Intentionally left empty...
        }

        public string GetTempPath() => "/tmp";

        public void DeleteFile(string path)
        {
            var entry = _files.FirstOrDefault(f => f.Path == path);
            if (entry != null)
            {
                _files.Remove(entry);
            }
        }

        public DateTime GetLastWriteTime(string path) => throw new NotImplementedException();

        public bool IsDirectory(string path) => throw new NotImplementedException();

        public string[] GetFiles(string path, string searchPattern) =>
            _files.Where(f => f.Path.StartsWith(path)).Select(f => f.Path).ToArray();

        public string GetCurrentDirectory() => throw new NotImplementedException();

        public DateTime GetModicationDateTime(string path) => throw new NotImplementedException();

        public void MoveFile(string oldPath, string newPath)
        {
            var entry = _files.FirstOrDefault(f => f.Path == oldPath);
            if (entry != null)
            {
                entry.Path = newPath;
            }
        }
    }
}
