using CQRS.Core.Commands;

namespace Post.Cmd.Application.Commands.Post.NewPostCommand
{
    public class NewPostCommand : BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
