using System;
using CaveTester.Core.DbSave;

namespace CaveTester.Core.Tester
{
    public abstract class Tester : IDisposable
    {
        protected readonly IdGenerator IdGenerator = new IdGenerator();
        protected readonly DbSaveHandler SaveHandler = new DbSaveHandler();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                IdGenerator.Reset();
                SaveHandler.RestoreAll();
                SaveHandler.DeleteAll();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
