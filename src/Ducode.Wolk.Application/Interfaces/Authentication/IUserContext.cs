namespace Ducode.Wolk.Application.Interfaces.Authentication
{
    public interface IUserContext
    {
        long CurrentUserId { get; }

        string SecurityStamp { get; }
    }
}
