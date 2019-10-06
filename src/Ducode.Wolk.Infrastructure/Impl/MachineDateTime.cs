using System;
using Ducode.Wolk.Common;

namespace Ducode.Wolk.Infrastructure.Impl
{
    internal class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
