using System.Collections.Generic;

namespace Ducode.Wolk.Domain.Entities
{
    public class Notebook : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Note> Notes { get; set; }
    }
}
