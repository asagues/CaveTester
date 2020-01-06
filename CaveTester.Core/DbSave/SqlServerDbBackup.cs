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

        public SqlServerDbBackup(DatabaseFacade database, string? saveDirectory = null)
        {
            _database = database;
            _databaseName = database.GetDbConnection().Database;
            _directory = saveDirectory ?? Directory.GetCurrentDirectory();
            Name = database.GetDbConnection().DataSource;
        }
        
        /// <inheritdoc />
        public void Create()
        {
            _database.ExecuteSqlCommand(CreateBackupSql);
        }

        /// <inheritdoc />
        public Task CreateAsync()
        {
            return _database.ExecuteSqlCommandAsync(CreateBackupSql);
        }

        /// <inheritdoc />
        public void Restore()
        {
            _database.ExecuteSqlCommand(RestoreBackupSql);
        }

        /// <inheritdoc />
        public Task RestoreAsync()
        {
            return _database.ExecuteSqlCommandAsync(RestoreBackupSql);
        }

        /// <inheritdoc />
        public void Delete()
        {
            _database.ExecuteSqlCommand(DeleteBackupSql);

            if (File.Exists(Path))
                File.Delete(Path); // delete orphan backup file
        }

        /// <inheritdoc />
        public async Task DeleteAsync()
        {
            await _database.ExecuteSqlCommandAsync(DeleteBackupSql);

            if (File.Exists(Path))
                File.Delete(Path); // delete orphan backup file
        }

        private string CreateBackupSql => "USE master;"
                                          + $"ALTER DATABASE {_databaseName} SET RECOVERY SIMPLE;"
                                          + $"BACKUP DATABASE {_databaseName} TO DISK = '{Path}' "
                                          + $"WITH FORMAT, NAME = '{BackupName}';";
        private string RestoreBackupSql => "USE MASTER;"
                                           + $"ALTER DATABASE {_databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;\r\n"
                                           + $"RESTORE DATABASE {_databaseName} FROM DISK = '{Path}' WITH REPLACE;"
                                           + $"ALTER DATABASE {_databaseName} SET MULTI_USER;";
        private string DeleteBackupSql => $"IF EXISTS(select * from sys.databases where name='{BackupName}') "
                                          + $"DROP DATABASE {BackupName}";
    }
}