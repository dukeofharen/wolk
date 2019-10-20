using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ducode.Wolk.Common;
using Newtonsoft.Json;

namespace Ducode.Wolk.Infrastructure.Impl
{
    // https://github.com/khellang/MimeTypes/blob/master/src/MimeTypes/MimeTypes.cs.pp
    internal class MimeService : IMimeService
    {
        private const string FallbackMimeType = "application/octet-stream";
        private static Dictionary<string, string> _typeMap;

        public MimeService()
        {
            InitializeTypeMap();
        }

        public string GetMimeType(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return FallbackMimeType;
            }

            return _typeMap.TryGetValue(input.Split('.').Last(), out var result) ? result : FallbackMimeType;
        }

        private void InitializeTypeMap()
        {
            var mimePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/mime.json");
            var contents = File.ReadAllText(mimePath);
            _typeMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);
        }
    }
}
