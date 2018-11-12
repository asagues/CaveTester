using Bogus;
using JetBrains.Annotations;

namespace CaveTester.Bogus.RuleSet
{
    public interface IRuleSet
    {
        [NotNull] Faker<T> Apply<T>([NotNull] Faker<T> faker)
            where T : class;
    }

    //Bogus already defines IRuleSet<T>
    public interface IRulesFor<T>
        where T : class
    {
        [NotNull] Faker<T> Apply([NotNull] Faker<T> faker);
    }
}