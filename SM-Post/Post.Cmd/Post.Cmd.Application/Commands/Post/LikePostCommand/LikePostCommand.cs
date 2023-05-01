using CQRS.Core.Commands;

namespace Post.Cmd.Application.Commands.Post.LikePostCommand
{
    public class LikePostCommand : BaseCommand
    {

        public Guid PostId { get; set; }
    }
}
