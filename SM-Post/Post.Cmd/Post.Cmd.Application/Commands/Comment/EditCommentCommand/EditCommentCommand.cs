using CQRS.Core.Commands;

namespace Post.Cmd.Application.Commands.Comment.EditCommentCommand
{
    public class EditCommentCommand : BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
    }
}
