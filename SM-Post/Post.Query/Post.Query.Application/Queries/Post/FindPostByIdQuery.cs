using CQRS.Core.Queries;

namespace Post.Query.Application.Queries.Post
{
    public class FindPostByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}