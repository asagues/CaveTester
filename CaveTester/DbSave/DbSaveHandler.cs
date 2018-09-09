using System.Collections.Generic;
using System.Data;

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
        public void Add(IDbSave save, string id = null)
        {
            id = id ?? save.Name;
            if (_databases.ContainsKey(id))
                return ;

            save.Initialize();
            save.Create();

            _databases.Add(id, save);
        }

        /// <summary>
        /// Restore a save
        /// </summary>
        /// <param name="id">The id of the save to restore</param>
        public void Restore(string id) => _databases[id].Restore();

        /// <summary>
        /// Equivalent to <code>Restore(save, connection.ConnectionString)</code>
        /// </summary>
        public void Restore(IDbConnection connection) => Restore(connection.ConnectionString);

        /// <summary>
        /// Restore all saves
        /// </summary>
        public void RestoreAll()
        {
            foreach (var save in _databases.Values)
                save.Restore();
        }

        /// <summary>
        /// Delete a save
        /// </summary>
        /// <param name="id">The id of the save to restore</param>
        public void Delete(string id)
        {
            _databases[id].Delete();
            _databases.Remove(id);
        }
        
        public void Delete(IDbConnection connection) => Delete(connection.ConnectionString);

        public void DeleteAll()
        {
            foreach (var save in _databases.Values)
                save.Delete();
            _databases.Clear();
        }
    }
}