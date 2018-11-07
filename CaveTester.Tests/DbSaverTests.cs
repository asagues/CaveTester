using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CaveTester.Core.DbSave;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CaveTester.Tests
{
    public class DbSaveTests
    {
        [Fact]
        public async Task ShouldSaveAndRestoreDatabase()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TestContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=(localdb)\\CaveTester;Database=CaveTester;Trusted_Connection=True;MultipleActiveResultSets=true");

            var context = new TestContext(dbContextOptions.Options);
            context.Database.EnsureCreated();

            var save = new SqlServerDbSnapshot(context.Database);
            await save.InitializeAsync();
            await save.CreateAsync();

            var entity = new Turret { IsDefective = true, };

            await context.Turrets.AddAsync(entity);
            await context.SaveChangesAsync();

            await save.RestoreAsync();

            var results = await context.Turrets.ToListAsync();

            results.Should().BeEmpty();
        }
    }

    //TODO move
    public class TestContext : DbContext
    {
        public DbSet<Turret> Turrets { get; set; }

        public TestContext([NotNull] DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        { }
    }

    //TODO move
    public class Turret
    {
        [Required] public int Id { get; set; }
        [Required] public bool IsDefective { get; set; }
    }
}