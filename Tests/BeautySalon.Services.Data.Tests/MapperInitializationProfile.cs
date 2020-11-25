namespace BeautySalon.Services.Data.Tests
{
    using System.Reflection;

    using AutoMapper;
    using BeautySalon.Services.Mapping;

    public class MapperInitializationProfile : Profile
    {
        public MapperInitializationProfile()
        {
            AutoMapperConfig.RegisterMappings(Assembly.GetCallingAssembly());
        }
    }
}
