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
    /// Abstract connector class for assigning attributes to a <see cref="Bundle"/>. The assignment can be implemented in <see cref="In(Bundle)"/> by setting the header of an incoming <see cref="Bundle"/>
    /// (see <see cref="Bundle.SetHeader(string, string)"/>). Use <see cref="StaticAttributeSetter"/> for assigning a <see cref="Bundle"/>-indpendent list of predefined attributes.
    /// </summary>
    public abstract class AttributeSetter : IPipe
    {
        #region Successor
        private IUnit _successor;

        /// <summary>
        /// Gets and sets the successor (target) of this connector
        /// </summary>
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


        public abstract void In(Bundle bundle);
        
    }
}
