using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateCommentAsync(CommentEntity comment);
        Task UpdateCommentAsync(CommentEntity comment);
        Task DeleteCommentAsync(Guid id);
        Task<CommentEntity> GetCommentByIdAsync(Guid id);
    }
}
