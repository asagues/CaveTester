using System.Threading.Tasks;

namespace CaveTester.Core.DbSave
{
    public interface IDbSave
    {
        /// <summary>
        /// The database name that this saves
        /// </summary>
        string Name { get; }

        /// <summary>
        /// If there is a save, restore and then delete it. This is needed because leftover save usually mean a test was interrupted and might have left the database in a dirty state
        /// </summary>
        void Initialize();
        /// <summary>
        /// If there is a save, restore and then delete it. This is needed because leftover save usually mean a test was interrupted and might have left the database in a dirty state
        /// </summary>
        Task InitializeAsync();
        /// <summary>
        /// Create a new save
        /// </summary>
        void Create();
        /// <summary>
        /// Create a new save
        /// </summary>
        Task CreateAsync();
        /// <summary>
        /// Restore from a save
        /// </summary>
        void Restore();
        /// <summary>
        /// Restore from a save
        /// </summary>
        Task RestoreAsync();
        /// <summary>
        /// Delete the save
        /// </summary>
        void Delete();
        /// <summary>
        /// Delete the save
        /// </summary>
        Task DeleteAsync();
    }
}