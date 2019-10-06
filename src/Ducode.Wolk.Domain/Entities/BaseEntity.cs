using System;

namespace Ducode.Wolk.Domain.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset Changed { get; set; }
    }
}
