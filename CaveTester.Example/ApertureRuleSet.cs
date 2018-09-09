using Bogus;
using CaveTester.Bogus.RuleSet;
using CaveTester.Example.Aperture;

namespace CaveTester.Example
{
    public class ApertureRuleSet : RuleSet, IRulesFor<Turret>
    {
        public Faker<Turret> Apply(Faker<Turret> faker)
        {
            return DefaultRules(faker)
                //Set the turret name that will be generated to a random lorem word
                .RuleFor(turret => turret.Name, f => f.Lorem.Word());
        }
    }
}