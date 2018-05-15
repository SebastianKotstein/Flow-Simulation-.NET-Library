using Skotstein.app.flowsimulation.lib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// Represents a <see cref="Worker"/> instance which process a <see cref="Bundle"/> by delaying it (the content is kept untouched). The duration of delaying can be set per <see cref="Bundle"/> or per <see cref="Entity"/> (but then muliplied by the number of processed <see cref=" Entity"/> object within one <see cref="Bundle"/>).
    /// Furthermore, the duration value (regardless of the mode whether it is per <see cref="Bundle"/> or per <see cref="Entity"/>) can be set statically such that it has the same value for all incoming <see cref="Bundle"/>s or can be set dynamically by reading a predefined header (attribute) of each incoming <see cref="Bundle"/>.
    /// Use <see cref="SetDuration(long, bool)"/> to set the duration mode and a static duration. Use <see cref="SetDuration(string, bool)"/> to set the duration mode and a dynamic duration by defining the header name which should be read.
    /// </summary>
    public class DelayWorker : Worker
    {
        private Bundle _currentBundle = null;

        #region Duration
        private long _duration;
        private bool _isDurationPerEntity = false;
        private bool _useDurationFromHeader = false;
        private string _durationHeader = "";

        private long _counter = 0;

        /// <summary>
        /// Gets the duration for processing (delaying) one <see cref="Bundle"/> or one <see cref="Entity"/> depending whether the <see cref="IsDurationPerEntity"/> is set to true
        /// </summary>
        public long Duration
        {
            get
            {
                return _duration;
            }
        }

        /// <summary>
        /// Indicates whether the duration is per processed <see cref="Bundle"/> or per <see cref="Entity"/>
        /// </summary>
        public bool IsDurationPerEntity
        {
            get
            {
                return _isDurationPerEntity;
            }

        }

        /// <summary>
        /// Sets the static duration value and duration mode.
        /// </summary>
        /// <param name="duration">duration</param>
        /// <param name="isDurationPerEntity">Indicates whether the duration is per processed <see cref="Bundle"/> (false) or per <see cref="Entity"/> (true)</param>
        public void SetDuration(long duration, bool isDurationPerEntity)
        {
            _useDurationFromHeader = false;
            _duration = duration;
            _isDurationPerEntity = isDurationPerEntity;
        }

        /// <summary>
        /// Sets the header name where the dynamic duration can be found and the duration mode.
        /// </summary>
        /// <param name="header">header name where the dynamic duration can be found</param>
        /// <param name="isDurationPerEntity">Indicates whether the duration is per processed <see cref="Bundle"/> (false) or per <see cref="Entity"/> (true)</param>
        public void SetDuration(string header, bool isDurationPerEntity)
        {
            _useDurationFromHeader = true;
            _durationHeader = header;
            _isDurationPerEntity = isDurationPerEntity;
        }
        #endregion

        /// <summary>
        /// Creates an instance of a <see cref="DelayWorker"/> and sets it ID.
        /// </summary>
        /// <param name="id"></param>
        public DelayWorker(string id) : base(id)
        {
        }

       

        public override void Update(long tick)
        {
            switch (State)
            {
                case WorkerState.idle:
                    //if worker has been paused
                    if(_currentBundle!= null)
                    {
                        State = WorkerState.busy;
                        goto case WorkerState.busy;
                    }
                    //if there are new Bundles available
                    if(Host.InputBufferCount > 0)
                    {
                        //grab one
                        _currentBundle = Host.Take();
                        long duration;
                        if (_useDurationFromHeader)
                        {
                            duration = Int64.Parse(_currentBundle.GetHeader(_durationHeader));
                        }
                        else
                        {
                            duration = Duration;
                        }

                        if (IsDurationPerEntity)
                        {
                            _counter = duration * _currentBundle.Count;
                        }
                        else
                        {
                            _counter = duration;
                        }
                        State = WorkerState.busy;
                        _counter--; //since this tick already counts
                    }
                    break;
                case WorkerState.busy:
                    _counter--;
                    if (_counter == 0)
                    {
                        //if completed, forward it to next hop
                        Host.Successor?.In(_currentBundle);
                        _currentBundle = null;
                        State = WorkerState.idle;
                    }
                    break;
                case WorkerState.blocked:
                    //not implemented
                    break;
                case WorkerState.paused:
                    
                    break;
            }
        }

        public override int Count
        {
            get
            {
                if(_currentBundle!= null)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public override int EntityCount
        {
            get
            {
                if (_currentBundle != null)
                {
                    return _currentBundle.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
