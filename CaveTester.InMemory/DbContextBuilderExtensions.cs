using CaveTester.Core.Tester;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.InMemory
{
    public static class DbContextBuilderExtensions
    {
        public static DbContextBuilder<TContext> WithInMemory<TContext>(this DbContextBuilder<TContext> builder, DbContextBuilder<TContext>.OptionBuilder optionsBuilder = null)
            where TContext : DbContext
        {
            var b = optionsBuilder == null
                                 ? (DbContextBuilder<TContext>.OptionBuilder)((o, connectionString) => o.UseInMemoryDatabase(connectionString))
                                 : (o, connectionString) => optionsBuilder(o.UseInMemoryDatabase(connectionString), connectionString);
            builder.With("InMemory", b);

            return builder;
        }
    }
}
