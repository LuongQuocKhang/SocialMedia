using CQRS.Core.Commands;

namespace Post.Cmd.Application.Commands.Message.EditMessageCommand
{
    public class EditMessageCommand : BaseCommand
    {
        public string Message { get; set; }
    }
}
