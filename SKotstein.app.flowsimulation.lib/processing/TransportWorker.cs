using Skotstein.app.flowsimulation.lib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// Represents a <see cref="TransportWorker"/> instance which process a <see cref="Bundle"/> by "transporting" (forwarding) it to the next hop (successor). Since a
    /// <see cref="TransportWorker"/> has to return back to its initial point for picking up the next <see cref="Bundle"/>, there is a second delay between the point of time of forwarding the
    /// currently processed <see cref="Bundle"/> and of picking the next <see cref="Bundle"/> for transportation. Both durations for transporting the <see cref="Bundle"/> and returning back to the
    /// initial point can be set statically such that is has the same value for all incoming <see cref="Bundle"/>s or can be set dynamically by reading a predefined header (attribute) of each incoming
    /// <see cref="Bundle"/>. Use <see cref="SetDurationAway(long)"/> and <see cref="SetDurationWayBack(long)"/> to set a static duration. Use <see cref="SetDurationAway(string)"/> and <see cref="SetDurationWayBack(string)"/> to set a dynamic duration by defining the
    /// header name which should be read.
    /// </summary>
    public class TransportWorker : Worker
    {
        private Bundle _currentBundle = null;

        #region Duration
        private long _durationAway;
        private long _durationWayBack;

        private bool _useDurationAwayFromHeader;
        private bool _useDurationWayBackFromHeader;

        private string _durationAwayHeaderName;
        private string _durationWayBackHeaderName;

        private long _counter = 0;

        /// <summary>
        /// Sets the static duration for transporting the <see cref="Bundle"/>
        /// </summary>
        /// <param name="duration">duration value</param>
        public void SetDurationAway(long duration)
        {
            _useDurationAwayFromHeader = false;
            _durationAway = duration;
        }
        /// <summary>
        /// Sets the static duration for returning back to the initial point before the next <see cref="Bundle"/> can be picked up.
        /// </summary>
        /// <param name="duration">duration value</param>
        public void SetDurationWayBack(long duration)
        {
            _useDurationWayBackFromHeader = false;
            _durationWayBack = duration;
        }
        /// <summary>
        /// Set the header name where the dynamic duration can be found for transporting the <see cref="Bundle"/>.
        /// </summary>
        /// <param name="headerName"></param>
        public void SetDurationAway(string headerName)
        {
            _useDurationAwayFromHeader = true;
            _durationAwayHeaderName = headerName;
        }
        /// <summary>
        /// Set the header name where the dynamic duration can be found for returning back to the initial point before the next <see cref="Bundle"/> can be picked up.
        /// </summary>
        /// <param name="headerName"></param>
        public void SetDurationWayBack(string headerName)
        {
            _useDurationWayBackFromHeader = true;
            _durationWayBackHeaderName = headerName;
        }
        #endregion

        /// <summary>
        /// Creates an instance of <see cref="TransportWorker"/> and sets it ID.
        /// </summary>
        /// <param name="id"></param>
        public TransportWorker(string id) : base(id)
        {
        }

        public override void Update(long tick)
        {
            switch (State)
            {
                case WorkerState.idle:
                    //if worker has been paused
                    if (_counter != 0)
                    {
                        State = WorkerState.busy;
                        goto case WorkerState.busy;
                    }
                    //if there are new Bundles available
                    if(Host.InputBufferCount > 0)
                    {
                        //grab one
                        _currentBundle = Host.Take();
                        long durationAway = _useDurationAwayFromHeader ? Int64.Parse(_currentBundle.GetHeader(_durationAwayHeaderName)) : _durationAway;
                        long durationWayBack = _useDurationWayBackFromHeader ? Int64.Parse(_currentBundle.GetHeader(_durationWayBackHeaderName)) : _durationWayBack;

                        _counter = durationAway + durationWayBack;
                        State = WorkerState.busy;
                        _counter--;
                    }
                    break;
                case WorkerState.busy:
                    _counter--;
                    if(_counter == 0)
                    {
                        State = WorkerState.idle;
                    }
                    if(_counter == (_useDurationWayBackFromHeader ? Int64.Parse(_currentBundle.GetHeader(_durationWayBackHeaderName)) : _durationWayBack))
                    {
                        //if first way (to target) completed, unload Bundle and return
                        Host.Successor?.In(_currentBundle);
                        _currentBundle = null;
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
                if (_currentBundle != null)
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
