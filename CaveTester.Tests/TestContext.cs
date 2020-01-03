using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Tests
{
    public class TestContext : DbContext
    {
        public DbSet<Turret> Turrets { get; set; }

        public TestContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        { }
    }

    public class Turret
    {
        [Required] public int Id { get; set; }
        [Required] public bool IsDefective { get; set; }
    }
}