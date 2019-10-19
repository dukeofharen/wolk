using System.Reflection;
using AutoMapper;
using Ducode.Wolk.Application.Infrastructure.AutoMapper;

namespace Ducode.Wolk.Api.Infrastructure.AutoMapper
{
    public class ApiAutoMapperProfile : Profile
    {
        public ApiAutoMapperProfile()
        {
            this.InitializeProfile(Assembly.GetExecutingAssembly());
        }
    }
}
