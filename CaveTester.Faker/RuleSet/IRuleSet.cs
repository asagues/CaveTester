using Bogus;

namespace CaveTester.Bogus.RuleSet
{
    public interface IRuleSet
    {
        Faker<T> Apply<T>(Faker<T> faker)
            where T : class;
    }

    //Bogus already define IRuleSet<T>
    public interface IRulesFor<T>
        where T : class
    {
        Faker<T> Apply(Faker<T> faker);
    }
}