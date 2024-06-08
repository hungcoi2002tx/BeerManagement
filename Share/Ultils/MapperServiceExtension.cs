using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Share.Ultils
{
    public static class MapperServiceExtension
    {
        public static void AddAutoMapperConfig(this IServiceCollection services)
        {
            //#region Add automapper
            //var assembly = Assembly.GetAssembly(typeof(MapperServiceExtension));
            //services.AddAutoMapper(assembly);
            //#endregion
            //services.AddAutoMapper(typeof(MapperConfig));
        }
    }
}
