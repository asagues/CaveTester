using System;
using CaveTester.Core.DbSave;
using JetBrains.Annotations;

namespace CaveTester.Core.Tester
{
    [PublicAPI]
    public abstract class Tester : IDisposable
    {
        protected static readonly DbSaveHandler SaveHandler = new DbSaveHandler();
        protected readonly IdGenerator IdGenerator = new IdGenerator();

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
