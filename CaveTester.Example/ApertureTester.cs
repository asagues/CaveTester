using CaveTester.Bogus.RuleSet;
using CaveTester.Core.DbSave;
using CaveTester.Core.Tester;
using CaveTester.Example.Aperture;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Example
{
    public abstract class ApertureTester : Tester
    {
        protected readonly ApertureContext Context;

        protected ApertureTester()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApertureContext>()
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=(localdb)\\CaveTester;Database=tests;Trusted_Connection=True;MultipleActiveResultSets=true");

            Context = new ApertureContext(dbContextOptions.Options);

            //Save the database
            //The database will automatically be restored from this save when the test end
            SaveHandler.Add(new SqlServerDbSnapshot(Context.Database));

            //Set which rule set to use when generating objects
            RuleSetHandler.RuleSet = new ApertureRuleSet();
        }
    }
}