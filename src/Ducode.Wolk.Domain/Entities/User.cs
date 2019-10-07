namespace Ducode.Wolk.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }
    }
}
