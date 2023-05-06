using Post.Query.Domain.Entities;

namespace Post.Common.DTOs.QueryResponse
{
    public class PostLookUpResponse : BaseResponse
    {
        public List<PostEntity> Posts { get; set; }
    }
}