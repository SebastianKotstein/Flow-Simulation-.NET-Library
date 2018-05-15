using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// A <see cref="Worker"/> can be considered as an executor for processing <see cref="Bundles"/>. Each worker is linked to an instance of <see cref="IFilter"/>. One instance of <see cref="IFilter"/> can have muliple
    /// <see cref="Worker"/>s, but one <see cref="Worker"/> cannot be shared among many <see cref="IFilter"/> instances.
    /// </summary>
    public abstract class Worker
    {
        #region State
        private WorkerState _state;
        /// <summary>
        /// Gets or sets the state
        /// </summary>
        public WorkerState State
        {
            get
            {
                return _state;
            }

            protected set
            {
                _state = value;
            }
        }
        #endregion
        #region ID
        private string _id;
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }


        #endregion
        #region Host
        private Processor _host;
        /// <summary>
        /// Gets or sets the host
        /// </summary>
        public Processor Host
        {
            get
            {
                return _host;
            }

            set
            {
                _host = value;
            }
        }
        #endregion
        /// <summary>
        /// Creates a <see cref="Worker"/> instance having the passed ID
        /// </summary>
        /// <param name="id">worker ID</param>
        public Worker(string id)
        {
            _id = id;
        }

        public abstract void Update(long tick);

        /// <summary>
        /// Pauses the underlying worker (sets the state to <see cref="WorkerState.paused"/>).
        /// </summary>
        public void Pause()
        {
            _state = WorkerState.paused;
        }

        /// <summary>
        /// Continues the work of the underlying worker (sets the state to <see cref="WorkerState.idle"/>).
        /// </summary>
        public void Continue()
        {
            _state = WorkerState.idle;
        }

        /// <summary>
        /// Gets the number of <see cref="Bundle"/>s which are currently processed
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Gets the number of <see cref="Entity"/> objects which are currently processed
        /// </summary>
        public abstract int EntityCount { get; }
    }

    public enum WorkerState
    {
        idle, busy, paused, blocked
    }
}
