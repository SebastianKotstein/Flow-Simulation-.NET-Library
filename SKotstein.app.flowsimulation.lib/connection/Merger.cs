using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skotstein.app.flowsimulation.lib.common;
using Skotstein.app.flowsimulation.lib.model;

namespace Skotstein.app.flowsimulation.lib.connection
{
    /// <summary>
    /// Connector class merging incoming <see cref="Bundle"/>s into one <see cref="Bundle"/>. The <see cref="Merger"/> waits until the required number of <see cref="Bundle"/>s has arrived before merging them to one <see cref="Bundle"/> and forwarding it to the <see cref="Successor"/>.
    /// The number of <see cref="Bundles"/> which should merged within one merger step can be defined in <see cref="Size"/>. Note that this value is related to the number of incoming <see cref="Bundle"/>s but not to the count of underlying <see cref="Entity"/> objects.
    /// Note that header (attributes) set in the original <see cref="Bundle"/> are lost after merging.
    /// </summary>
    public class Merger : IPipe
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
        #region Size
        private int _size;
        /// <summary>
        /// Gets or sets the number of <see cref="Bundle"/> which should be merged to one <see cref="Bundle"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }
        #endregion
        #region Merging
        private Queue<Bundle> _waitingBuffer = new Queue<Bundle>();
        #endregion
        /// <summary>
        /// Creates a new instance of <see cref="Merger"/> and defines the number of <see cref="Bundle"/>s which should be merged to one <see cref="Bundle"/>.
        /// </summary>
        /// <param name="size">number of bundles to be merged</param>
        public Merger(int size)
        {
            _size = size;
        }



        public void In(Bundle bundle)
        {
            _waitingBuffer.Enqueue(bundle);
            if(_waitingBuffer.Count >= Size)
            {
                Bundle b = new Bundle();
                for(int i = 0; i < Size; i++)
                {
                    Bundle bb = _waitingBuffer.Dequeue();
                    foreach(Entity e in bb.Entities)
                    {
                        b.AddEntity(e);
                    }
                }
                Successor?.In(b);
            }
        }
    }
}
