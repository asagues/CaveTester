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
        public void Create() { }

        /// <inheritdoc />
        public void Restore() { }

        /// <inheritdoc />
        public void Delete() { }
    }
}