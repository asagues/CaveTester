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