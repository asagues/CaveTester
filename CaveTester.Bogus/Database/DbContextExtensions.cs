using System;
using Bogus;
using CaveTester.Bogus.RuleSet;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Bogus.Database
{
    public static class DbContextExtensions
    {
        public static DbContext Generate<T>(this DbContext dbContext, int quantity = 1, Action<Faker, T> rules = null, Action<Faker<T>> config = null)
            where T : class
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Can not generate less then one object");

            var faker = new Faker<T>().ApplyGlobalRuleSet();

            config?.Invoke(faker);
            if (rules != null)
                faker.Rules(rules);

            dbContext.AddRange(faker.Generate(quantity));

            return dbContext;
        }
    }
}