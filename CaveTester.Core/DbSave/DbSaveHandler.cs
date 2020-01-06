using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CaveTester.Core.DbSave
{
    public class DbSaveHandler
    {
        private readonly IDictionary<string, IDbSave> _databases = new Dictionary<string, IDbSave>();

        /// <summary>
        /// Initialize and create a save.
        /// If a save with <param name="id">id</param> already exists, does nothing
        /// </summary>
        /// <param name="save">The save to add</param>
        /// <param name="id">The save id, if null save.Name is used</param>
        [PublicAPI] 
        public void Add(IDbSave save, string? id = default)
        {
            id ??= save.Name;
            if (_databases.ContainsKey(id))
                return ;

            save.Create();

            _databases.Add(id, save);
        }

        /// <summary>
        /// Initialize and create a save.
        /// If a save with <param name="id">id</param> already exists, does nothing
        /// </summary>
        /// <param name="save">The save to add</param>
        /// <param name="id">The save id, if null save.Name is used</param>
        [PublicAPI]
        public async Task AddAsync(IDbSave save, string? id = null)
        {
            id ??= save.Name;
            if (_databases.ContainsKey(id))
                return ;

            await save.CreateAsync();

            _databases.Add(id, save);
        }

        /// <summary>
        /// Restore a save
        /// </summary>
        /// <param name="id">The id of the save to restore</param>
        [PublicAPI]
        public void Restore(string id) => _databases[id].Restore();

        /// <summary>
        /// Restore a save
        /// </summary>
        /// <param name="id">The id of the save to restore</param>
        [PublicAPI]
        public Task RestoreAsync(string id) => _databases[id].RestoreAsync();

        /// <summary>
        /// Restore a save
        /// Equivalent to <c>Restore(save, connection.ConnectionString)</c>
        /// </summary>
        [PublicAPI]
        public void Restore(IDbConnection connection) => Restore(connection.ConnectionString);

        /// <summary>
        /// Restore a save
        /// Equivalent to <c>RestoreAsync(save, connection.ConnectionString)</c>
        /// </summary>
        [PublicAPI]
        public Task RestoreAsync(IDbConnection connection) => RestoreAsync(connection.ConnectionString);

        /// <summary>
        /// Restore all saves
        /// </summary>
        [PublicAPI]
        public void RestoreAll()
        {
            foreach (var save in _databases.Values)
                save.Restore();
        }

        /// <summary>
        /// Restore all saves
        /// </summary>
        [PublicAPI]
        public Task RestoreAllAsync()
        {
            var tasks = new Task[_databases.Values.Count];

            foreach (var save in _databases.Values)
                tasks.Append(save.RestoreAsync());

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// Delete a save
        /// </summary>
        /// <param name="id">The id of the save to restore</param>
        [PublicAPI]
        public void Delete(string id)
        {
            _databases[id].Delete();
            _databases.Remove(id);
        }

        /// <summary>
        /// Delete a save
        /// </summary>
        /// <param name="id">The id of the save to restore</param>
        [PublicAPI]
        public async Task DeleteAsync(string id)
        {
            await _databases[id].DeleteAsync();
            _databases.Remove(id);
        }
        
        /// <summary>
        /// Delete a save
        /// Equivalent to <c>Delete(save, connection.ConnectionString)</c>
        /// </summary>
        [PublicAPI]
        public void Delete(IDbConnection connection) => Delete(connection.ConnectionString);

        /// <summary>
        /// Delete a save
        /// Equivalent to <c>DeleteAsync(save, connection.ConnectionString)</c>
        /// </summary>
        [PublicAPI]
        public Task DeleteAsync(IDbConnection connection) => DeleteAsync(connection.ConnectionString);

        /// <summary>
        /// Delete all saves
        /// </summary>
        [PublicAPI]
        public void DeleteAll()
        {
            foreach (var save in _databases.Values)
                save.DeleteAsync();

            _databases.Clear();
        }

        /// <summary>
        /// Delete all saves
        /// </summary>
        [PublicAPI]
        public async Task DeleteAllAsync()
        {
            var tasks = new Task[_databases.Values.Count];

            foreach (var save in _databases.Values)
                tasks.Append(save.DeleteAsync());

            await Task.WhenAll(tasks);

            _databases.Clear();
        }
    }
}