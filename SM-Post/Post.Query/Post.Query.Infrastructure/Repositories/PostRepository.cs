
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreatePostAsync(PostEntity post)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                context.Posts.Add(post);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeletePostAsync(Guid id)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                PostEntity post = await GetPostByIdAsync(id).ConfigureAwait(false);
                if(post == null) return;

                context.Posts.Remove(post);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdatePostAsync(PostEntity post)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                context.Posts.Update(post);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<PostEntity>> GetAllPostsAsync()
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                List<PostEntity> post = await context.Posts.AsNoTracking()
                                                            .Include(t => t.Comments).AsNoTracking()
                                                            .ToListAsync()
                                                            .ConfigureAwait(false);

                return post;
            }
        }

        public async Task<List<PostEntity>> GetPostsByAuthorAsync(string author)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                List<PostEntity> post = await context.Posts.AsNoTracking()
                                                            .Include(t => t.Comments).AsNoTracking()
                                                            .Where(x => x.Author == author)
                                                            .ToListAsync()
                                                            .ConfigureAwait(false);

                return post;
            }
        }

        public async Task<PostEntity> GetPostByIdAsync(Guid id)
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                PostEntity post = await context.Posts.Include(t => t.Comments)
                                                .FirstOrDefaultAsync(x => x.PostId == id)
                                                .ConfigureAwait(false);

                return post;
            }
        }

        public async Task<List<PostEntity>> GetPostsWithCommentsAsync()
        {
            using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                List<PostEntity> post = await context.Posts.AsNoTracking()
                                                            .Include(t => t.Comments).AsNoTracking()
                                                            .Where(x => x.Comments != null && x.Comments.Any())
                                                            .ToListAsync()
                                                            .ConfigureAwait(false);

                return post;
            }
        }

        public async Task<List<PostEntity>> GetPostsWithLikesAsync(int numberOfLikes)
        {
           using(DatabaseContext context = _contextFactory.CreateDbContext()) 
            {
                List<PostEntity> post = await context.Posts.AsNoTracking()
                                                            .Include(t => t.Comments).AsNoTracking()
                                                            .Where(x => x.Likes >= numberOfLikes)
                                                            .ToListAsync()
                                                            .ConfigureAwait(false);

                return post;
            }
        } 
    }
}