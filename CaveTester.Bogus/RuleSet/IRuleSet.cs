using Bogus;
using JetBrains.Annotations;

namespace CaveTester.Bogus.RuleSet
{
    [PublicAPI]
    public interface IRuleSet
    {
        Faker<T> Apply<T>(Faker<T> faker)
            where T : class;
    }

    //Bogus already defines IRuleSet<T>
    [PublicAPI]
    public interface IRulesFor<T>
        where T : class
    {
        Faker<T> Apply(Faker<T> faker);
    }
}