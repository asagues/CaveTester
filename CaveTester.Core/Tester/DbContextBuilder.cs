using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Core.Tester
{
    [PublicAPI]
    public class DbContextBuilder<TContext>
        where TContext : DbContext
    {
        private readonly Database _database;
        private OptionBuilder? _optionBuilder;

        internal DbContextBuilder(Database database)
        {
            _database = database;
        }

        public DbContextBuilder<TContext> With(string provider, OptionBuilder optionBuilder)
        {
            if (string.Compare(provider, _database.Provider, StringComparison.InvariantCultureIgnoreCase) != 0)
                return this;

            _optionBuilder = optionBuilder;

            return this;
        }

        public TContext Build()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            if (_optionBuilder == null)
                throw new ArgumentOutOfRangeException($"No provider configured for {_database.Provider}. Use .{nameof(With)}(\"{_database.Provider}\") to connect to your database", (Exception?) null);

            var options = _optionBuilder(optionsBuilder, _database.ConnectionString).Options;

            return (TContext) Activator.CreateInstance(typeof(TContext), options);
        }

        public delegate DbContextOptionsBuilder<TContext> OptionBuilder(DbContextOptionsBuilder<TContext> builder, string connectionString);
    }
}