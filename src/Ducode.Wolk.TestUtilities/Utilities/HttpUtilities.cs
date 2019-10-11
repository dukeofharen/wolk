using System.Net.Http;

namespace Ducode.Wolk.TestUtilities.Utilities
{
    public static class HttpUtilities
    {
        public static void AddJwtBearer(this HttpRequestMessage message, string jwt) =>
            message.Headers.Add("Authorization", $"Bearer {jwt}");
    }
}
