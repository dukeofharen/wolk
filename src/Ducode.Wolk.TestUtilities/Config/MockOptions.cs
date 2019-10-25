using Microsoft.Extensions.Options;

namespace Ducode.Wolk.TestUtilities.Config
{
    public class MockOptions<T> : IOptions<T> where T : class, new()
    {
        private MockOptions(T config)
        {
            Value = config;
        }

        public static MockOptions<T> Create(T config) => new MockOptions<T>(config);

        public T Value { get; }
    }
}
