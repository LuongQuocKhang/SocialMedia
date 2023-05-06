using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<PostEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handler = new();
        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handler.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("You can't register the same query handler twice!");
            }
            _handler.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<PostEntity>> SendAsync(BaseQuery query)
        {
            if (_handler.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<PostEntity>>> handler))
            {
                return await handler(query);
            }
            throw new ArgumentNullException(nameof(handler),"No query handler was registered!");
        }
    }
}