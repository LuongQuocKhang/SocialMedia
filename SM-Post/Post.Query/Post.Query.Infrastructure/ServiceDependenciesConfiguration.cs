
using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Infrastructure
{
    public static class ServiceDependenciesConfiguration
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository,CommentRepository>();

            services.AddScoped<IEventHandler, Handlers.EventHandler>();
            services.AddScoped<IEventConsumer, EventConsumer>();
            return services;
        }

    }
}