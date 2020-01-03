using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
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
            _database.ExecuteSqlRaw($"CREATE DATABASE {SnapshotName} ON\r\n"
                                    + $"( NAME = {_databaseName}, FILENAME = '{SnapshotPath}' )\r\n"
                                    + $"AS SNAPSHOT OF {_databaseName};\r\n");
        }

        /// <inheritdoc />
        public Task CreateAsync()
        {
            return _database.ExecuteSqlRawAsync($"CREATE DATABASE {SnapshotName} ON\r\n"
                                                + $"( NAME = {_databaseName}, FILENAME = '{SnapshotPath}' )\r\n"
                                                + $"AS SNAPSHOT OF {_databaseName};\r\n");
        }

        /// <inheritdoc />
        public void Restore()
        {
            _database.ExecuteSqlRaw("USE MASTER;\r\n"
                                    + $"ALTER DATABASE {_databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;\r\n"
                                    + $"RESTORE DATABASE {_databaseName} FROM\r\n"
                                    + $"DATABASE_SNAPSHOT = '{SnapshotName}';\r\n"
                                    + $"ALTER DATABASE {_databaseName} SET MULTI_USER;\r\n");
        }

        /// <inheritdoc />
        public Task RestoreAsync()
        {
            return _database.ExecuteSqlRawAsync("USE MASTER;\r\n"
                                                + $"ALTER DATABASE {_databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;\r\n"
                                                + $"RESTORE DATABASE {_databaseName} FROM\r\n"
                                                + $"DATABASE_SNAPSHOT = '{SnapshotName}';\r\n"
                                                + $"ALTER DATABASE {_databaseName} SET MULTI_USER;\r\n");
        }

        /// <inheritdoc />
        public void Delete()
        {
            _database.ExecuteSqlRaw("DROP DATABASE " + SnapshotName);

            if (File.Exists(SnapshotPath))
                File.Delete(SnapshotPath); // delete orphan snapshot file
        }

        /// <inheritdoc />
        public async Task DeleteAsync()
        {
            await _database.ExecuteSqlRawAsync("DROP DATABASE " + SnapshotName);

            if (File.Exists(SnapshotPath))
                File.Delete(SnapshotPath); // delete orphan snapshot file
        }
    }
}