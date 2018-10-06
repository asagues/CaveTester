using System.Threading.Tasks;
using CaveTester.Bogus.Database;
using CaveTester.Bogus.RuleSet;
using CaveTester.Core;
using CaveTester.Core.DbSave;
using CaveTester.Example.Aperture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CaveTester.Example
{
    public class UnabstractedExampleTest
    {
        //flat / unabstracted example of a test
        [Fact]
        public async Task ShouldDeleteTurrets()
        {
            //Arrange

            //Setup database connection
            var dbContextOptions = new DbContextOptionsBuilder<ApertureContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=(localdb)\\CaveTester;Database=tests;Trusted_Connection=True;MultipleActiveResultSets=true");

            var dbContext = new ApertureContext(dbContextOptions.Options);

            //Create a snapshot of the database
            var saveHandler = new DbSaveHandler();
            saveHandler.Add(new SqlServerDbSnapshot(dbContext.Database));

            //Set wich rule set to use when generating objects
            RuleSetHandler.RuleSet = new ApertureRuleSet();

            var idGenerator = new IdGenerator();

            var turretService = new TurretService(dbContext);
            // Generate 42 turrets
            // The ruleset set in ApertureTester constructor is applied, so the turrets names will have a value
            await dbContext.Generate<Turret>(42, (faker, turret) =>
                {
                    //Get a new unique turret id
                    turret.Id = idGenerator.New<Turret>();
                    turret.IsDefective = faker.Random.Bool();
                })
                .SaveChangesAsync();

            //Act
            await turretService.RemoveAllDefectiveTurrets();

            //Assert
            var haveDefectiveTurret = await dbContext.Turrets.AnyAsync(turret => turret.IsDefective);
            haveDefectiveTurret.Should().BeTrue();
        }
    }
}