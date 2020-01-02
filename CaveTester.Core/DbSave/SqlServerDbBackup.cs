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
    public class SqlServerDbBackup : IDbSave
    {
        public string Name { get; }

        private readonly DatabaseFacade _database;
        private readonly string _directory;
        private readonly string _databaseName;

        private string BackupName => _databaseName + "_backup";
        private string Path => System.IO.Path.Combine(_directory, _databaseName + ".bak");

        public SqlServerDbBackup(DatabaseFacade database)
        {
            _database = database;
            _databaseName = database.GetDbConnection().Database;
            _directory = Directory.GetCurrentDirectory();
            Name = database.GetDbConnection().DataSource;
        }

        /// <inheritdoc />
        public void Initialize()
        {
            //Try to restore from backup to ensure the database is in a clean state, then delete it
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
            //Try to restore from backup to ensure the database is in a clean state, then delete it
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
            _database.ExecuteSqlCommand($"USE {_databaseName};"
                                        + $"BACKUP DATABASE {_databaseName} TO DISK = '{Path}' "
                                        + $"WITH FORMAT, MEDIANAME = 'Z_SQLServerBackups', NAME = '{BackupName}';");
        }

        /// <inheritdoc />
        public Task CreateAsync()
        {
            return _database.ExecuteSqlCommandAsync($"USE {_databaseName};"
                                                    + $"BACKUP DATABASE {_databaseName} TO DISK = '{Path}' "
                                                    + $"WITH FORMAT, MEDIANAME = 'Z_SQLServerBackups', NAME = '{BackupName}';");
        }

        /// <inheritdoc />
        public void Restore()
        {
            _database.ExecuteSqlCommand("USE MASTER;\r\n"
                                        + $"ALTER DATABASE {_databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;\r\n"
                                        + $"RESTORE DATABASE {_databaseName} FROM DISK = '{Path}'"
                                        + $"ALTER DATABASE {_databaseName} SET MULTI_USER;\r\n");
        }

        /// <inheritdoc />
        public Task RestoreAsync()
        {
            return _database.ExecuteSqlCommandAsync("USE MASTER;\r\n"
                                                    + $"ALTER DATABASE {_databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;\r\n"
                                                    + $"RESTORE DATABASE {_databaseName} FROM DISK = '{Path}'"
                                                    + $"ALTER DATABASE {_databaseName} SET MULTI_USER;\r\n");
        }

        /// <inheritdoc />
        public void Delete()
        {
            _database.ExecuteSqlCommand($"IF EXISTS(select * from sys.databases where name='{BackupName}') "
                                        + $"DROP DATABASE {BackupName}");

            if (File.Exists(Path))
                File.Delete(Path); // delete orphan backup file
        }

        /// <inheritdoc />
        public async Task DeleteAsync()
        {
            await _database.ExecuteSqlCommandAsync($"IF EXISTS(select * from sys.databases where name='{BackupName}') "
                                                   + $"DROP DATABASE {BackupName}");

            if (File.Exists(Path))
                File.Delete(Path); // delete orphan backup file
        }
    }
}