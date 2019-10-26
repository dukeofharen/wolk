using System.IO;
using Ducode.Wolk.Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Ducode.Wolk.Api.Utilities
{
    public static class StartupUtilities
    {
        public static IApplicationBuilder UseGui(this IApplicationBuilder app, bool loadStaticFiles)
        {
            if (!loadStaticFiles)
            {
                return app;
            }

            var path = $"{AssemblyHelper.GetCallingAssemblyRootPath()}/gui";
            if (Directory.Exists(path))
            {
                app.UseFileServer(new FileServerOptions
                {
                    EnableDefaultFiles = true,
                    FileProvider = new PhysicalFileProvider(path)
                });
            }

            return app;
        }
    }
}
