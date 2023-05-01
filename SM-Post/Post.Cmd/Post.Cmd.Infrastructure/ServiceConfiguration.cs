using CQRS.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Post.Cmd.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();

            return services;
        }
    }
}
