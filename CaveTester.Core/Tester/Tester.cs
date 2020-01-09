using System;
using System.IO;
using System.Text.Json;
using CaveTester.Core.DbSave;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CaveTester.Core.Tester
{
    [PublicAPI]
    public abstract class Tester : IDisposable
    {
        protected static readonly DbSaveHandler SaveHandler = new DbSaveHandler();
        protected readonly IdGenerator IdGenerator = new IdGenerator();
        protected readonly Database Database;

        static Tester()
        {
            AppDomain.CurrentDomain.DomainUnload += delegate
            {
                SaveHandler.DeleteAll();
            };
            AppDomain.CurrentDomain.ProcessExit += delegate
            {
                SaveHandler.DeleteAll();
            };
        }

        protected Tester(string? configPath = null)
        {
            var rawConfig = File.ReadAllText(configPath ?? "appsettings.json");
            Database = JsonSerializer.Deserialize<AppSettings>(rawConfig).Cycle.Database;
        }

        protected Tester(AppSettings.CycleSettings settings)
        {
            Database = settings.Database;
        }

        protected DbContextBuilder<TContext> AddContext<TContext>()
            where TContext : DbContext
        {
            return new DbContextBuilder<TContext>(Database);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IdGenerator.Reset();
                SaveHandler.RestoreAll();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
