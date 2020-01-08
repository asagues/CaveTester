using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Example.Aperture
{
    public class ApertureContext : DbContext
    {
        public DbSet<Turret> Turrets { get; set; }

        public ApertureContext([NotNull] DbContextOptions<ApertureContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        { }
    }
}