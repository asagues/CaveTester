using CaveTester.Core.Tester;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.SqlServer
{
    public static class DbContextBuilderExtensions
    {
        public static DbContextBuilder<TContext> WithSqlServer<TContext>(this DbContextBuilder<TContext> builder, DbContextBuilder<TContext>.OptionBuilder? optionsBuilder = null)
            where TContext : DbContext
        {
            var sqlBuilder = (DbContextBuilder<TContext>.OptionBuilder) ((o, connectionString) => o.UseSqlServer(connectionString));

            optionsBuilder += sqlBuilder;

            builder.With("SqlServer", optionsBuilder);

            return builder;
        }
    }
}