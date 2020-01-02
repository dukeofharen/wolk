using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Ducode.Wolk.Common.Utilities
{
    public static class ZipExtensions
    {
        public static ZipArchiveEntry AddEntry(this ZipArchive zip, string filename, byte[] data)
        {
            var entry = zip.CreateEntry(filename);
            using var entryStream = entry.Open();
            entryStream.Write(data, 0, data.Length);
            return entry;
        }

        public static ZipArchiveEntry AddEntry(this ZipArchive zip, string filename, string data) =>
            zip.AddEntry(filename, Encoding.UTF8.GetBytes(data));

        public static byte[] ReadEntry(this ZipArchive zip, string filename, bool throwOnNotFound = true)
        {
            var entry = zip.Entries.FirstOrDefault(e => e.Name == filename);
            if (entry == null)
            {
                if (throwOnNotFound)
                {
                    throw new InvalidOperationException($"No ZIP entry found for file '{filename}'.");
                }

                return null;
            }

            using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            entryStream.CopyTo(ms);
            return ms.ToArray();
        }

        public static string ReadEntryAsText(this ZipArchive zip, string filename, bool throwOnNotFound = true)
        {
            var result = zip.ReadEntry(filename, throwOnNotFound);
            return result == null ? null : Encoding.UTF8.GetString(result);
        }
    }
}
