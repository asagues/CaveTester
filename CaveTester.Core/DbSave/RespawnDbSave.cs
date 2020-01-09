using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Respawn;

namespace CaveTester.Core.DbSave
{
    /// <inheritdoc />
    [PublicAPI]
    public class RespawnDbSave : IDbSave
    {
        private Checkpoint? _checkpoint;
        private readonly string _connectionString;

        public RespawnDbSave(DatabaseFacade database)
        {
            _connectionString = database.GetDbConnection().ConnectionString;
            Name = database.GetDbConnection().ConnectionString;
        }

        public string Name { get; }
        
        /// <inheritdoc />
        public void Create() => CreateAsync().Wait();

        /// <inheritdoc />
        public Task CreateAsync()
        {
            _checkpoint = new Checkpoint();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Restore() => RestoreAsync().Wait();

        /// <inheritdoc />
        public async Task RestoreAsync()
        {
            await _checkpoint.Reset(_connectionString);
        }

        /// <inheritdoc />
        public void Delete() { }

        /// <inheritdoc />
        public Task DeleteAsync() => Task.CompletedTask;
    }
}