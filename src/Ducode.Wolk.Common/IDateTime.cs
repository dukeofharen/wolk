using System;

namespace Ducode.Wolk.Common
{
    public interface IDateTime
    {
        DateTimeOffset Now { get; }

        DateTimeOffset UtcNow { get; }
    }
}
