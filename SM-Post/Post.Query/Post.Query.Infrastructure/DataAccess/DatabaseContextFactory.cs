using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext ?? throw new ArgumentNullException(nameof(configureDbContext));
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            _configureDbContext(optionBuilder);

            return new DatabaseContext(optionBuilder.Options);
        }
    }
}
