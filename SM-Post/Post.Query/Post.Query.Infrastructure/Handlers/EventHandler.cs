using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Common.Events.Comment;
using Post.Common.Events.Message;
using Post.Common.Events.Post;
using Post.Query.Domain.Entities;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;

        public EventHandler(PostRepository postRepository, CommentRepository commentRepository) 
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task On(PostCreatedEvent @event)
        {
            var post = new PostEntity()
            {
                PostId = @event.Id,
                Author = @event.Author,
                Message = @event.Message,
                CreatedDate = @event.CreatedDate
            };

            await _postRepository.CreatePostAsync(post).ConfigureAwait(false);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            PostEntity post = await _postRepository.GetPostByIdAsync(@event.Id);
            if(post == null) return;

            post.Message = @event.Message;
            await _postRepository.UpdatePostAsync(post).ConfigureAwait(false);
        }

        public async Task On(PostLikedEvent @event)
        {
            PostEntity post = await _postRepository.GetPostByIdAsync(@event.Id);
            if(post == null) return;

            post.Likes++;
            await _postRepository.UpdatePostAsync(post).ConfigureAwait(false);
        }

        public async Task On(CommentAddedEvent @event)
        {
            CommentEntity comment = new CommentEntity() {
                CommentId = @event.CommentId,
                PostId = @event.Id,
                Comment = @event.Comment,
                Username = @event.Username,
                CreatedDate = @event.CreatedDate
            };
            await _commentRepository.CreateCommentAsync(comment).ConfigureAwait(false);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            CommentEntity comment = await _commentRepository.GetCommentByIdAsync(@event.CommentId);
            if(comment == null) return;

            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.CreatedDate = @event.CreatedDate;

            await _commentRepository.UpdateCommentAsync(comment).ConfigureAwait(false);
        }

        public async Task On(CommentRemovedEvent @event)
        {
            await _commentRepository.DeleteCommentAsync(@event.CommentId).ConfigureAwait(false);
        }

        public async Task On(PostRemovedEvent @event)
        {
            await _postRepository.DeletePostAsync(@event.Id).ConfigureAwait(false);
        }
    }
}