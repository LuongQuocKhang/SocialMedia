using CQRS.Core.Events;

namespace Post.Common.Events.Post
{
    public class PostRemovedEvent : BaseEvent
    {
        public PostRemovedEvent() : base(nameof(PostRemovedEvent))
        {
        }
    }
}
