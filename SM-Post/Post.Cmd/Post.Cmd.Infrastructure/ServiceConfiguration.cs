using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Post.Cmd.Application.Commands;
using Post.Cmd.Application.Commands.Comment.AddCommentCommand;
using Post.Cmd.Application.Commands.Comment.EditCommentCommand;
using Post.Cmd.Application.Commands.Comment.RemoveCommentComand;
using Post.Cmd.Application.Commands.Message.EditMessageCommand;
using Post.Cmd.Application.Commands.Post.DeletePostCommand;
using Post.Cmd.Application.Commands.Post.LikePostCommand;
using Post.Cmd.Application.Commands.Post.NewPostCommand;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddPostCmdServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventStore, EventStore>();

            services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();

            services.AddScoped<ICommandHandler, CommandHandler>();



            var commandHandler = services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            var dispatcher = new CommandDispatcher();

            dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RemoveCommentComand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);

            services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

            return services;
        }
    }
}
