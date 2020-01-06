using System;
using System.Reflection;
using Bogus;
using JetBrains.Annotations;

namespace CaveTester.Bogus.RuleSet
{
    /// <summary>
    /// Setup rules set to generate objects with Faker
    /// To use, inherit this class and implement <see cref="IRulesFor{T}"/> for each type
    /// </summary>
    [PublicAPI]
    public class RuleSet : IRuleSet
    {
        /// <summary>
        /// If <see cref="IRulesFor{T}"/> is implemented for T, use rules from <see cref="IRulesFor{T}.Apply"/>, otherwise use <see cref="DefaultRules{T}"/>
        /// </summary>
        public Faker<T> Apply<T>(Faker<T> faker)
            where T : class
        {
            if (this is IRulesFor<T> rules)
                rules.Apply(faker);
            else
                InvokeGenericMethod(nameof(DefaultRules), faker);

            return faker;
        }

        protected virtual Faker<T> DefaultRules<T>(Faker<T> faker)
            where T : class
            => faker;

        private object InvokeGenericMethod<T>(string methodName, Faker<T> faker, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic)
            where T : class
        {
            var method = GetType().GetMethod(methodName, bindingFlags);
            if (method == null)
                throw new MissingMethodException(nameof(RuleSet), nameof(methodName));

            return method.MakeGenericMethod(typeof(T))
                .Invoke(this, new object[] {faker});
        }
    }
}