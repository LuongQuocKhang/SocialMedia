﻿using CQRS.Core.Events;

namespace Post.Common.Events.Post
{
    public class PostCreatedEvent : BaseEvent
    {
        public PostCreatedEvent() : base(nameof(PostCreatedEvent))
        {
        }
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
