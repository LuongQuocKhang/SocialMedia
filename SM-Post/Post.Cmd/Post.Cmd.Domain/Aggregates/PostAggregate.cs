using CQRS.Core.Domain;
using Post.Common.Events.Comment;
using Post.Common.Events.Message;
using Post.Common.Events.Post;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        #region Properties
        private bool _active;

        private string _author;

        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new Dictionary<Guid, Tuple<string, string>>();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {

        }
        #endregion

        #region Edit Message
        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post!");
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"the value of {nameof(message)} can not be null or empty. Please provide a valid {nameof(message)}!");
            }

            RaiseEvent(new MessageUpdatedEvent()
            {
                Id = _id,
                CreatedDate = DateTime.Now,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }
        #endregion

        #region Create Post
        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent()
            {
                Id = id,
                Author = author,
                Message = message,
                CreatedDate = DateTime.Now,
                PostId = Guid.NewGuid(),
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }
        #endregion

        #region Like Post
        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post!");
            }

            RaiseEvent(new PostLikedEvent()
            {
                Id = _id,
                CreatedDate = DateTime.Now
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }
        #endregion

        #region Remove Post
        public void RemovePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Post already removed!");
            }

            if(!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post that was made by another user!");
            }

            RaiseEvent(new PostRemovedEvent()
            {
                Id = _id,
            });
        }
        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
        #endregion

        #region Add Comment
        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add a comment to an inactive post!");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"the value of {nameof(comment)} can not be null or empty. Please provide a valid {nameof(comment)}!");
            }

            RaiseEvent(new CommentAddedEvent()
            {
                Id = _id,
                CreatedDate = DateTime.Now,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }
        #endregion

        #region Edit Comment
        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot update a comment to an inactive post!");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"the value of {nameof(comment)} can not be null or empty. Please provide a valid {nameof(comment)}!");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to edit a comment that was made by another user.");
            }

            RaiseEvent(new CommentUpdatedEvent()
            {
                Id = _id,
                CreatedDate = DateTime.Now,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }
        #endregion

        #region Remove Comment
        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment to an inactive post!");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException($"You are not allowed to remove a comment that was made by another user.");
            }

            RaiseEvent(new CommentRemovedEvent()
            {
                Id = _id,
                CreatedDate = DateTime.Now,
                CommentId = Guid.NewGuid()
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }
        #endregion
    }
}
