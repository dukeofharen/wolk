using AutoMapper;
using Ducode.Wolk.Application.Infrastructure.AutoMapper;

namespace Ducode.Wolk.TestUtilities.Mapping
{
    public class MapperFactory
    {
        public static IMapper GetMapper() =>
            new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationAutoMapperProfile())).CreateMapper();
    }
}
