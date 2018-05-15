using Skotstein.app.flowsimulation.lib.common;
using Skotstein.app.flowsimulation.lib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// Repository for storing incoming <see cref="Bundle"/>s. Stored <see cref="Bundle"/> objects can be taken out by applying the FIFO principle by calling <see cref="Take"/> for a single object or <see cref="Take(int)"/> for multiple objects.
    /// </summary>
    public class Repository : IUnit
    {
        #region Successor (not existing)
        public IUnit Successor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
        #region Repository
        private Queue<Bundle> _repository = new Queue<Bundle>();

        /// <summary>
        /// Returns the number of <see cref="Bundle"/>s in this repository
        /// </summary>
        public int Count
        {
            get
            {
                return _repository.Count;
            }
        }
        /// <summary>
        /// Returns and removes the next <see cref="Bundle"/> out of the <see cref="Repository"/> or returns null if the <see cref="Repository"/> is empty
        /// </summary>
        /// <returns></returns>
        public Bundle Take()
        {
            if(_repository.Count> 0)
            {
                return _repository.Dequeue();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns and removes multiple <see cref="Bundle"/>. The maximum size is specified over the first parameter but the actual count is limited by 
        /// number of <see cref="Bundle"/>s in the <see cref="Repository"/>
        /// </summary>
        /// <param name="count">preferred number of <see cref="Bundle"/>s</param>
        /// <returns>list with <see cref="Bundle"/>s</returns>
        public IList<Bundle> Take(int count)
        {
            IList<Bundle> bundles = new List<Bundle>();
            int counter = 0;
            while (counter < count && Count > 0)
            {
                bundles.Add(_repository.Dequeue());
                counter++;
            }
            return bundles;
        }
        #endregion



        public void In(Bundle bundle)
        {
            _repository.Enqueue(bundle);
        }
    }
}
