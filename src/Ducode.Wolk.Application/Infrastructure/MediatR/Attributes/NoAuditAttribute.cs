using System;

namespace Ducode.Wolk.Application.Infrastructure.MediatR.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NoAuditAttribute : Attribute
    {
    }
}
