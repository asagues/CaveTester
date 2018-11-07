using System.Threading.Tasks;

namespace CaveTester.Core.DbSave
{
    //TODO
    //In memory database are assumed to be unique, so there is no need to save/restore
    /// <inheritdoc />
    public class InMemoryDbSave : IDbSave
    {
        public InMemoryDbSave(string name)
        {
            Name = name;
        }

        public string Name { get; }

        /// <inheritdoc />
        public void Initialize() { }

        /// <inheritdoc />
        public Task InitializeAsync() => Task.CompletedTask;

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