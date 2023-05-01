using CQRS.Core.Events;

namespace Post.Common.Events.Comment
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent(string type) : base(nameof(CommentAddedEvent))
        {
        }
        public Guid CommentId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        
    }
}
