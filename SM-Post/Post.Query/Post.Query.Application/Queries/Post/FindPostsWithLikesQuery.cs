
using CQRS.Core.Queries;

namespace Post.Query.Application.Queries.Post
{
    public class FindPostsWithLikesQuery : BaseQuery
    {
        public int NumberOfLikes { get; set; }
    }
}