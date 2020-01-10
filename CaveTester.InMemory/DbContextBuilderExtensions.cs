using CaveTester.Core.Tester;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.InMemory
{
    public static class DbContextBuilderExtensions
    {
        public static DbContextBuilder<TContext> WithInMemory<TContext>(this DbContextBuilder<TContext> builder, DbContextBuilder<TContext>.OptionBuilder optionsBuilder = null)
            where TContext : DbContext
        {
            var inMemoryBuilder = (DbContextBuilder<TContext>.OptionBuilder) ((o, connectionString) => o.UseInMemoryDatabase(connectionString));

            optionsBuilder += inMemoryBuilder;

            builder.With("InMemory", optionsBuilder);

            return builder;
        }
    }
}
