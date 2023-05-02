using Post.Cmd.Domain.Entities;

namespace Post.Cmd.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(Guid id);

        Task<PostEntity> GetByIdAsync(Guid id);
        Task<List<PostEntity>> GetAllAsync();
        Task<List<PostEntity>> GetByAuthorAsync(string author);
        Task<List<PostEntity>> GetPostsWithLikesAsync(int numberOfLikes);
        Task<List<PostEntity>> GetPostsWithCommentsAsync();
    }
}
