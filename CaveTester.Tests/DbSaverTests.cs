using System.Threading.Tasks;
using CaveTester.Core.DbSave;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CaveTester.Tests
{
    public class DbSaveTests
    {
        [Fact]
        public async Task ShouldSaveAndRestoreWithRespawn()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TestContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=localhost;Database=tests;Trusted_Connection=True;");

            var context = new TestContext(dbContextOptions.Options);
            context.Database.EnsureCreated();

            var save = new RespawnDbSave(context.Database);
            await save.InitializeAsync();
            await save.CreateAsync();

            var entity = new Turret { IsDefective = true, };

            await context.Turrets.AddAsync(entity);
            await context.SaveChangesAsync();

            await save.RestoreAsync();

            var results = await context.Turrets.ToListAsync();

            results.Should().BeEmpty();

            await save.DeleteAsync();
        }

        [Fact]
        public async Task ShouldSaveAndRestoreWithBackup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TestContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=localhost;Database=tests;Trusted_Connection=True;");

            var context = new TestContext(dbContextOptions.Options);
            context.Database.EnsureCreated();

            var save = new SqlServerDbBackup(context.Database, "C:\\Temp");
            await save.InitializeAsync();
            await save.CreateAsync();

            var entity = new Turret { IsDefective = true, };

            await context.Turrets.AddAsync(entity);
            await context.SaveChangesAsync();

            await save.RestoreAsync();

            var results = await context.Turrets.ToListAsync();

            results.Should().BeEmpty();

            await save.DeleteAsync();
        }

        [Fact]
        public async Task ShouldSaveAndRestoreSnapshot()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TestContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=localhost;Database=tests;Trusted_Connection=True;");

            var context = new TestContext(dbContextOptions.Options);
            context.Database.EnsureCreated();

            var save = new SqlServerDbSnapshot(context.Database, "C:\\Temp");
            await save.InitializeAsync();
            await save.CreateAsync();

            var entity = new Turret { IsDefective = true, };

            await context.Turrets.AddAsync(entity);
            await context.SaveChangesAsync();

            await save.RestoreAsync();

            var results = await context.Turrets.ToListAsync();

            results.Should().BeEmpty();

            await save.DeleteAsync();
        }
    }
}