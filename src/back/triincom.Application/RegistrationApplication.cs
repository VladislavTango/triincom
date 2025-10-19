using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace triincom.Application
{
    public static class RegistrationApplication
    {
        public static IServiceCollection AddMapperServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
