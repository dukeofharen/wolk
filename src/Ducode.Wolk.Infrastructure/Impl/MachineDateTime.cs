using System;
using Ducode.Wolk.Common;

namespace Ducode.Wolk.Infrastructure.Impl
{
    internal class MachineDateTime : IDateTime
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
