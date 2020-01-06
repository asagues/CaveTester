using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CaveTester.Core.DbSave
{
    /// <inheritdoc />
    [PublicAPI]
    public class SqlServerDbSnapshot : IDbSave
    {
        public string Name { get; }

        private readonly DatabaseFacade _database;
        private readonly string _snapshotDirectory;
        private readonly string _databaseName;

        private string SnapshotName => _databaseName + "_snapshot";
        private string SnapshotPath => Path.Combine(_snapshotDirectory, _databaseName + ".ss");

        public SqlServerDbSnapshot(DatabaseFacade database, string? saveDirectory = null)
        {
            _database = database;
            _databaseName = database.GetDbConnection().Database;
            _snapshotDirectory = saveDirectory ?? Directory.GetCurrentDirectory();
            Name = database.GetDbConnection().DataSource;
        }

        /// <inheritdoc />
        public void Initialize()
        {
            //Try to restore from snapshot to ensure the database is in a clean state, then delete it
            try
            {
                Restore();
                Delete();
            }
            catch (SqlException)
            {
                //Exceptions are expected when there is not snapshot to restore
            }
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            //Try to restore from snapshot to ensure the database is in a clean state, then delete it
            try
            {
                await RestoreAsync();
                await DeleteAsync();
            }
            catch (SqlException)
            {
                //Exceptions are expected when there is not snapshot to restore
            }
        }

        /// <inheritdoc />
        public void Create()
        {
            _database.ExecuteSqlCommand(CreateSnapshotSql);
        }

        /// <inheritdoc />
        public Task CreateAsync()
        {
            return _database.ExecuteSqlCommandAsync(CreateSnapshotSql);
        }

        /// <inheritdoc />
        public void Restore()
        {
            _database.ExecuteSqlCommand(RestoreSnapshotSql);
        }

        /// <inheritdoc />
        public Task RestoreAsync()
        {
            return _database.ExecuteSqlCommandAsync(RestoreSnapshotSql);
        }

        /// <inheritdoc />
        public void Delete()
        {
            _database.ExecuteSqlCommand(DeleteSnapshotSql);

            if (File.Exists(SnapshotPath))
                File.Delete(SnapshotPath); // delete orphan snapshot file
        }

        /// <inheritdoc />
        public async Task DeleteAsync()
        {
            await _database.ExecuteSqlCommandAsync(DeleteSnapshotSql);

            if (File.Exists(SnapshotPath))
                File.Delete(SnapshotPath); // delete orphan snapshot file
        }

        private string CreateSnapshotSql => $"CREATE DATABASE {SnapshotName} ON"
                                            + $"( NAME = {_databaseName}, FILENAME = '{SnapshotPath}' )"
                                            + $"AS SNAPSHOT OF {_databaseName};";
        private string RestoreSnapshotSql => "USE MASTER;"
                                             + $"ALTER DATABASE {_databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;"
                                             + $"RESTORE DATABASE {_databaseName} FROM DATABASE_SNAPSHOT = '{SnapshotName}';"
                                             + $"ALTER DATABASE {_databaseName} SET MULTI_USER;";

        private string DeleteSnapshotSql => $"DROP DATABASE {SnapshotName}";
    }
}