using Post.Query.Application.Queries.Post;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Application.handlers
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _postRepository;

        public QueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
        {
            return await _postRepository.GetAllPostsAsync().ConfigureAwait(false);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var post = await _postRepository.GetPostByIdAsync(query.Id).ConfigureAwait(false);
            return new List<PostEntity>() {
                post
            };
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
        {
            return await _postRepository.GetPostsByAuthorAsync(query.Author).ConfigureAwait(false);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
        {
            return await _postRepository.GetPostsWithCommentsAsync().ConfigureAwait(false);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
        {
            return await _postRepository.GetPostsWithLikesAsync(query.NumberOfLikes).ConfigureAwait(false);
        }
    }
}