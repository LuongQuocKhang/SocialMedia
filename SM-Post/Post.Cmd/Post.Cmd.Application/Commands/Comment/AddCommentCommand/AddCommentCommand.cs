using CQRS.Core.Commands;

namespace Post.Cmd.Application.Commands.Comment.AddCommentCommand
{
    public class AddCommentCommand : BaseCommand
    {
        public string Username { get; set; }
        public string Comment { get; set; }
    }
}
