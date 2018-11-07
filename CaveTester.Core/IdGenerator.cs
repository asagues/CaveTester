using System;
using System.Collections.Generic;

namespace CaveTester.Core
{
    /// <summary>
    /// Determinist id generator
    /// </summary>
    // TODO create an IIdGenerator and handle any kind of id
    public sealed class IdGenerator
    {
        private readonly IDictionary<Type, int> _ids = new Dictionary<Type, int>();

        /// <summary>
        /// Generate a new id. Generated ids are guaranteed to be unique and sequential for <typeparam name="T">T</typeparam>
        /// </summary>
        /// <returns>Returns the next id for T</returns>
        public int New<T>()
        {
            if (_ids.TryGetValue(typeof(T), out var id))
            {
                _ids[typeof(T)] = id + 1;
                return id;
            }

            _ids.Add(typeof(T), 2);
            return 1;
        }

        /// <summary>
        /// Reset the id counter for <typeparam name="T">T</typeparam>
        /// </summary>
        public void Reset<T>() => _ids.Remove(typeof(T));

        /// <summary>
        /// Reset all id counters
        /// </summary>
        public void Reset() => _ids.Clear();
    }
}
