using CQRS.Core.Handlers;
using Post.Cmd.Application.Commands.Comment.AddCommentCommand;
using Post.Cmd.Application.Commands.Comment.EditCommentCommand;
using Post.Cmd.Application.Commands.Comment.RemoveCommentComand;
using Post.Cmd.Application.Commands.Message.EditMessageCommand;
using Post.Cmd.Application.Commands.Post.DeletePostCommand;
using Post.Cmd.Application.Commands.Post.LikePostCommand;
using Post.Cmd.Application.Commands.Post.NewPostCommand;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Application.Commands.Handlers.Post
{
    public class PostCommandHandler : IPostCommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

        public PostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler ?? throw new ArgumentNullException(nameof(eventSourcingHandler));
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.AddComment(command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.EditComment(command.CommentId, command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentComand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.RemoveComment(command.CommentId, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.EditMessage(command.Message);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.RemovePost(command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.LikePost();

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RestoreReadDbCommand command)
        {
            await _eventSourcingHandler.RepublishEventsAsync();
        }
    }
}
