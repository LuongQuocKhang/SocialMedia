using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreatePostAsync(PostEntity post);
        Task UpdatePostAsync(PostEntity post);
        Task DeletePostAsync(Guid id);

        Task<PostEntity> GetPostByIdAsync(Guid id);
        Task<List<PostEntity>> GetAllPostsAsync();
        Task<List<PostEntity>> GetPostsByAuthorAsync(string author);
        Task<List<PostEntity>> GetPostsWithLikesAsync(int numberOfLikes);
        Task<List<PostEntity>> GetPostsWithCommentsAsync();
    }
}
