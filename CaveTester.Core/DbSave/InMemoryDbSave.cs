using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CaveTester.Core.DbSave
{
    //In memory database are assumed to be unique, so there is no need to save/restore
    /// <inheritdoc />
    [PublicAPI]
    public class InMemoryDbSave : IDbSave
    {
        public InMemoryDbSave(string name)
        {
            Name = name;
        }

        public string Name { get; }

        /// <inheritdoc />
        public void Create() { }

        /// <inheritdoc />
        public Task CreateAsync() => Task.CompletedTask;

        /// <inheritdoc />
        public void Restore() { }

        /// <inheritdoc />
        public Task RestoreAsync() => Task.CompletedTask;

        /// <inheritdoc />
        public void Delete() { }

        /// <inheritdoc />
        public Task DeleteAsync() => Task.CompletedTask;
    }
}