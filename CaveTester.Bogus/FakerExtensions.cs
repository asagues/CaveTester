using Bogus;

namespace CaveTester.Bogus
{
    public static class FakerExtensions
    {
        /// <summary>
        /// Fluent method for setting <see cref="Faker{T}.Locale"/> 
        /// </summary>
        public static Faker<T> SetLocale<T>(this Faker<T> faker, string locale = "en-us")
            where T : class
        {
            faker.Locale = locale;

            return faker;
        }
    }
}