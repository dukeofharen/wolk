using System;

namespace Ducode.Wolk.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Authorization failed")
        {
        }
    }
}
