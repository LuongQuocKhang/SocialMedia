using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString)
        {
            Action<DbContextOptionsBuilder> configureDbContext = x => x
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString);

            services.AddDbContext<DatabaseContext>(configureDbContext);
            services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

            var dataContext = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
            dataContext.Database.EnsureCreated();
            
            return services;
        }
    }
}
