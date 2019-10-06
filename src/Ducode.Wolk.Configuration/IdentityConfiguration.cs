namespace Ducode.Wolk.Configuration
{
    public class IdentityConfiguration
    {
        public string JwtSecret { get; set; }

        public int ExpirationInSeconds { get; set; } = 3600;
    }
}
