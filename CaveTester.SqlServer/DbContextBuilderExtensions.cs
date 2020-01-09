using CaveTester.Core.Tester;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.SqlServer
{
    public static class DbContextBuilderExtensions
    {
        public static DbContextBuilder<TContext> WithSqlServer<TContext>(this DbContextBuilder<TContext> builder, DbContextBuilder<TContext>.OptionBuilder optionsBuilder = null)
            where TContext : DbContext
        {
            var b = optionsBuilder == null
                        ? (DbContextBuilder<TContext>.OptionBuilder)((o, connectionString) => o.UseSqlServer(connectionString))
                        : (o, connectionString) => optionsBuilder(o.UseSqlServer(connectionString), connectionString);
            builder.With("SqlServer", b);

            return builder;
        }
    }
}
