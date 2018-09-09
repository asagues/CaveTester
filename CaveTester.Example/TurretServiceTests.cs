using System.Threading.Tasks;
using CaveTester.Bogus.Database;
using CaveTester.Example.Aperture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CaveTester.Example
{
    public class TurretServiceTests : ApertureTester
    {
        private readonly TurretService _turretService;

        public TurretServiceTests()
        {
            _turretService = new TurretService(Context);
        }

        [Fact]
        public async Task ShouldRemoveDefectiveTurrets()
        {
            // Generate 42 turrets
            // The ruleset set in ApertureTester constructor is applied, so the turrets names will have a value
            await Context.Generate<Turret>(42, (faker, turret) =>
                {
                    //Get a new unique turret id
                    turret.Id = IdGenerator.New<Turret>();
                    turret.IsDefective = faker.Random.Bool();
                })
                .SaveChangesAsync();

            await _turretService.RemoveAllDefectiveTurrets();

            (await Context.Turrets.AllAsync(turret => !turret.IsDefective))
                .Should().BeTrue();
        }
    }
}