using Post.Cmd.Application.Commands.Comment.AddCommentCommand;
using Post.Cmd.Application.Commands.Comment.EditCommentCommand;
using Post.Cmd.Application.Commands.Comment.RemoveCommentComand;
using Post.Cmd.Application.Commands.Message.EditMessageCommand;
using Post.Cmd.Application.Commands.Post.DeletePostCommand;
using Post.Cmd.Application.Commands.Post.LikePostCommand;
using Post.Cmd.Application.Commands.Post.NewPostCommand;

namespace Post.Cmd.Application.Commands.Handlers.Post
{
    public interface IPostCommandHandler
    {
        Task HandleAsync(AddCommentCommand command);
        Task HandleAsync(EditCommentCommand command);
        Task HandleAsync(RemoveCommentComand command);
        Task HandleAsync(EditMessageCommand command);
        Task HandleAsync(DeletePostCommand command);
        Task HandleAsync(LikePostCommand command);
        Task HandleAsync(NewPostCommand command);
    }
}
