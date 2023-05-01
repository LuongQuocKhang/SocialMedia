using CQRS.Core.Commands;
namespace Post.Cmd.Application.Commands.Comment.RemoveCommentComand
{
    public class RemoveCommentComand : BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
