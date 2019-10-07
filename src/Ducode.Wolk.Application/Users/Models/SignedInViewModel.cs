namespace Ducode.Wolk.Application.Users.Models
{
    public class SignedInViewModel
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
