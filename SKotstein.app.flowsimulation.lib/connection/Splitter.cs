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
    /// Connector class splitting an incoming <see cref="Bundle"/> into multiple <see cref="Bundle"/>s by creating new <see cref="Bundle"/> objects and re-distributing the <see cref="Entity"/> objects
    /// of the origin <see cref="Bundle"/>. The preferred size of a new <see cref="Bundle"/> can be set in the constructor <see cref="Splitter(int)"/> or by using <see cref="SeekSize"/>.
    /// Since it is not necessary that <see cref="SeekSize"/> is a divider of the total <see cref="Entity"/> count of the origin <see cref="Bundle"/> wihtout any remainder. The <see cref="Entity"/> count of the last generated 
    /// <see cref="Bundle"/> of one split process might be smaller than the <see cref="SeekSize"/> (this <see cref="Bundle"/> contains the remaining <see cref="Entity"/> objects).
    /// Example: The <see cref="SeekSize"/> is 30. An incoming <see cref="Bundle"/> having 100 <see cref="Entity"/> objects is split into three <see cref="Bundle"/> with 30 <see cref="Entity"/> objects each and an additional
    /// <see cref="Bundle"/> containing the remaining 10 <see cref="Entity"/> objects.
    /// Note that header (attributes) set in the original <see cref="Bundle"/> are lost after splitting.
    /// </summary>
    public class Splitter : IPipe
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
        private int _seekSize;
        /// <summary>
        /// Gets or sets the preferred size for the splitted <see cref="Bundle"/>s.
        /// </summary>
        public int SeekSize
        {
            get
            {
                return _seekSize;
            }

            set
            {
                _seekSize = value;
            }
        }
        #endregion
        /// <summary>
        /// Creates a new instance of <see cref="Splitter"/> and sets the preferred size for the splitted <see cref="Bundle"/>s.
        /// </summary>
        /// <param name="seekSize">preferred size</param>
        public Splitter(int seekSize)
        {
            _seekSize = seekSize;
        }

        public void In(Bundle bundle)
        {
            //extract entities out of incoming bundle:
            IList<Entity> entities = bundle.Entities;

            //prepare new sub-bundles:
            Bundle subBundle = new Bundle();
            int counter = 0;

            for (int i = 0; i < entities.Count; i++)
            {
                //add carrier to new sub bundle
                subBundle.AddEntity(entities[i]);
                counter++;

                //until the limit for a sub bundle is reached:
                if (counter == SeekSize)
                {
                    //forward sub bundle
                    _successor?.In(subBundle);

                    //reset counter and create new sub bundle
                    counter = 0;
                    subBundle = new Bundle();
                }
            }
            //for the case that there are no more carriers left, but the last sub bundle has not reached the seeked size
            if (subBundle.Count > 1)
            {
                //forward sub bundle (even it has not the seeked size)
                _successor?.In(subBundle);
            }
        }
    }
}
