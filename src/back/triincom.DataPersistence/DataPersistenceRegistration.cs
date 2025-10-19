using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using triincom.DataPersistence.AppContext;

namespace triincom.DataPersistence
{
    public static class DataPersistenceRegistration
    {
        private const string ProjectDbConnectionString = "ProjectDbSection:ProjectDbConnection";

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnection = configuration.GetSection(ProjectDbConnectionString).Value;

            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dbConnection));

            return services;
        }
    }
}
