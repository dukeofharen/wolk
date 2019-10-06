using System.Reflection;
using AutoMapper;

namespace Ducode.Wolk.Application.Infrastructure.AutoMapper
{
    public class ApplicationAutoMapperProfile : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            this.InitializeProfile(Assembly.GetExecutingAssembly());
        }
    }
}
