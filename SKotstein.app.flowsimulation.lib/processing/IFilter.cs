using Skotstein.app.flowsimulation.lib.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// Interface definition for all filters including the definition of worker management
    /// </summary>
    public interface IFilter : IUnit
    {
        void Update(long tick);

        #region Worker Management
        /// <summary>
        /// Returns the number of existing worker
        /// </summary>
        int WorkerCount { get; }

        /// <summary>
        /// Returns the <see cref="Worker"/> object at the specified index
        /// </summary>
        /// <param name="index">index</param>
        /// <returns><see cref="Worker"/>object</returns>
        Worker GetWorkerByIndex(int index);

        /// <summary>
        /// Returns the <see cref="Worker"/> object having the passed ID or null if no such <see cref="Worker"/> object exists
        /// </summary>
        /// <param name="workerId">worker ID</param>
        /// <returns><see cref="Worker"/> object or null</returns>
        Worker GetWorkerById(string workerId);

        /// <summary>
        /// Adds a worker
        /// </summary>
        /// <param name="worker"><see cref="Worker"/> object to be added</param>
        void AddWorker(Worker worker);

        /// <summary>
        /// Returns and removes a worker at the specified index
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>removed <see cref="Worker"/> object</returns>
        Worker RemoveWorkerAt(int index);

        /// <summary>
        /// Returns and removes the worker having the passed ID or returns null if no such <see cref="Worker"/> exists.
        /// </summary>
        /// <param name="workerId">worker ID</param>
        /// <returns>removed <see cref="Worker"/>object or null</returns>
        Worker RemoveWorkerById(string workerId);

        /// <summary>
        /// Checks whether the <see cref="Worker"/> having the passed ID exists
        /// </summary>
        /// <param name="workerId">worker ID</param>
        /// <returns>true if the <see cref="Worker"/> object having the passed ID exists, else false</returns>
        bool HasWorker(string workerId);
        #endregion
        

    }
}
