namespace Ducode.Wolk.Api.Tests
{
    public abstract partial class IntegrationTestBase
    {
        public void EnsureFileExists(string path, string contents = "") => MockFileService.WriteAllText(path, contents);

        public void EnsureFileExists(string path, byte[] contents) => MockFileService.WriteAllBytes(path, contents);
    }
}
