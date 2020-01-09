using CaveTester.Bogus.RuleSet;
using CaveTester.Core.DbSave;
using CaveTester.Core.Tester;
using CaveTester.Example.Aperture;
using CaveTester.InMemory;
using CaveTester.SqlServer;

namespace CaveTester.Example
{
    public abstract class ApertureTester : Tester
    {
        protected readonly ApertureContext Context;

        protected ApertureTester()
        {
            Context = AddContext<ApertureContext>()
                      .WithSqlServer()
                      .WithInMemory()
                      .Build();


            //Save the database
            //The database will automatically be restored from this save when the test end
            SaveHandler.Add(new SqlServerDbSnapshot(Context.Database));

            //Set which rule set to use when generating objects
            RuleSetHandler.RuleSet = new ApertureRuleSet();
        }
    }
}