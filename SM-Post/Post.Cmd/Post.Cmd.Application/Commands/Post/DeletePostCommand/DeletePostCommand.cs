using CQRS.Core.Commands;

namespace Post.Cmd.Application.Commands.Post.DeletePostCommand
{
    public class DeletePostCommand : BaseCommand
    {
        public string Username { get; set; }
    }
}
