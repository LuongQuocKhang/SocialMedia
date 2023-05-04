
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }


        public async Task CreateCommentAsync(CommentEntity comment)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                context.Comments.Add(comment);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {

                CommentEntity comment = await GetCommentByIdAsync(id).ConfigureAwait(false);

                if (comment == null) return;

                context.Comments.Remove(comment);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<CommentEntity> GetCommentByIdAsync(Guid id)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext())
            {
                CommentEntity comment = await context.Comments.FirstOrDefaultAsync(x => x.CommentId == id).ConfigureAwait(false);
                return comment;
            }
        }

        public async Task UpdateCommentAsync(CommentEntity comment)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext())
            {
                context.Comments.Update(comment);
                await context.SaveChangesAsync();
            }
        }
    }
}