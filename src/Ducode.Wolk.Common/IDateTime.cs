using System;

namespace Ducode.Wolk.Common
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
