using CQRS.Core.Queries;

namespace Post.Query.Application.Queries.Post
{
    public class FindPostsByAuthorQuery : BaseQuery
    {
        public string Author { get; set; }
    }
}