using CQRS.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Application.Commands.Comment.RemoveCommentComand
{
    public class RemoveCommentComand : BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
