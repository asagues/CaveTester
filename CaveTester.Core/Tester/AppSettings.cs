using JetBrains.Annotations;

namespace CaveTester.Core.Tester
{
    [PublicAPI]
    public class AppSettings
    {
        public CycleSettings Cycle { get; set; }

        public class CycleSettings
        {
            public Database Database { get; set; }
        }
    }

    [PublicAPI]
    public class Database
    {
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
    }
}