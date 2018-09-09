namespace CaveTester.Core.DbSave
{
    public interface IDbSave
    {
        /// <summary>
        /// The database name that this saves
        /// </summary>
        string Name { get; }

        /// <summary>
        /// If there is a save, restore and then delete it. This needed because leftover save usually mean a test was interupted an might have left the database in a dirty state
        /// </summary>
        void Initialize();
        /// <summary>
        /// Create a new save
        /// </summary>
        void Create();
        /// <summary>
        /// Restore from a save
        /// </summary>
        void Restore();
        /// <summary>
        /// Delete the save
        /// </summary>
        void Delete();
    }
}