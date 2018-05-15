using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skotstein.app.flowsimulation.lib.model;
using Skotstein.app.flowsimulation.lib.model;
using Skotstein.app.flowsimulation.lib.common;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// A <see cref="Processor"/> partially implements the <see cref="IFilter"/> interface but does not specify how incoming <see cref="Entity"/> are processed.
    /// Hence, this abstract class, especially the <see cref="Update(long)"/> method must be completed for instanciating this class and its sub classes.
    /// </summary>
    public abstract class Processor : IFilter
    {

        #region Successor
        private IUnit _successor;

        public IUnit Successor
        {
            get
            {
                return _successor;
            }
            set
            {
                _successor = value;
            }
        }
        #endregion
        #region Input Buffer Management
        private Queue<Bundle> _inputBuffer = new Queue<Bundle>();
        private int _inputBufferLimit = -1;

        /// <summary>
        /// Gets or sets the limit of the input buffer. If the value is -1 the input buffer is unlimited
        /// </summary>
        public int InputBufferLimit
        {
            get
            {
                return _inputBufferLimit;
            }

            set
            {
                _inputBufferLimit = value;
            }
        }

        public void In(Bundle bundle)
        {
            if(_inputBufferLimit!=-1 && _inputBufferLimit < _inputBuffer.Count + 1)
            {
                throw new InputBufferOvervlowException(this,bundle);
            }
            _inputBuffer.Enqueue(bundle);
        }

        /// <summary>
        /// Gets the number of <see cref="Bundle"/> in the input buffer
        /// </summary>
        public int InputBufferCount
        {
            get
            {
                return _inputBuffer.Count;
            }
        }

        /// <summary>
        /// Gets the sum of all <see cref="Entity"/> objects in the input buffer
        /// </summary>
        public int InputBufferEntityCount
        {
            get
            {
                int sum = 0;
                foreach(Bundle b in _inputBuffer)
                {
                    sum += b.Count;
                }
                return sum;

            }
        }


        /// <summary>
        /// Returns and removes at least i <see cref="Bundle"/> objects from the input buffer (depdending) on the amount of stored <see cref="Entity"/> objects
        /// </summary>
        /// <param name="i">requested amount of <see cref="Entity"/> objects</param>
        /// <returns>list with <see cref="Entity"/> objects</returns>
        public List<Bundle> Take(int i)
        {
            List<Bundle> list = new List<Bundle>();
            int counter = 0;
            while (counter < i && InputBufferCount > 0)
            {
                list.Add(_inputBuffer.Dequeue());
            }
            return list;
        }

        /// <summary>
        /// Returns and removes the next <see cref="Bundle"/> object from the input buffer or returns null if the input buffer is empty
        /// </summary>
        /// <returns>next <see cref="Bundle"/> object or null</returns>
        public Bundle Take()
        {

            if (InputBufferCount > 0)
            {
                return _inputBuffer.Dequeue();
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Worker Management
        private IList<Worker> _worker = new List<Worker>();

        public int WorkerCount
        {
            get
            {
               return _worker.Count;
            }
        }

        protected IList<Worker> Worker
        {
            get
            {
                return _worker;
            }

        }

        public Worker GetWorkerByIndex(int index)
        {
            if(index < 0 || index >= WorkerCount)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return _worker[index];
            }
        }

        public Worker GetWorkerById(string workerId)
        {
            foreach(Worker w in _worker)
            {
                if (w.Id.CompareTo(workerId) == 0)
                {
                    return w;
                }
            }
            return null;
        }

        public void AddWorker(Worker worker)
        {
            worker.Host = this;
            _worker.Add(worker);
        }

        public Worker RemoveWorkerAt(int index)
        {
            if (index < 0 || index >= WorkerCount)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                Worker w = _worker[index];
                _worker.RemoveAt(index);
                return w;
            }
        }

        public Worker RemoveWorkerById(string workerId)
        {
            Worker w = null;
            for(int i = 0; i < _worker.Count; i++)
            {
                if (_worker[i].Id.CompareTo(workerId) == 0)
                {
                    w = _worker[i];
                    _worker.RemoveAt(i);
                    return w;
                }
            }
            return null;
        }

        public bool HasWorker(string workerId)
        {
            return GetWorkerById(workerId) != null;
        }
        #endregion


        public abstract void Update(long tick);

        /// <summary>
        /// Gets the number of <see cref="Bundle"/>s which are currently processed
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Gets the number of <see cref="Entity"/> objects which are currently processed
        /// </summary>
        public abstract int EntityCount { get; }

    }
}
