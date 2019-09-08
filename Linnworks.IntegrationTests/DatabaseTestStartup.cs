using System;
using LinnworksTest.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Linnworks.IntegrationTests.Core
{
    public static class DatabaseTestStartup
    {
        public static IConfiguration Configuration { get; }
        
        static DatabaseTestStartup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();
        }

        public static IServiceProvider ConfigureServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddDbContext<CategoriesManagementContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("LinnworksDatabase")));
            

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITokenRepository, TokenRepository>();
            return services.BuildServiceProvider();
        }
    }
}
