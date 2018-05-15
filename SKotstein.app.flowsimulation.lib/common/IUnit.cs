using Skotstein.app.flowsimulation.lib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.common
{
    /// <summary>
    /// Root type for pipes and filters
    /// </summary>
    public interface IUnit
    {
        /// <summary>
        /// Inserts a <see cref="Bundle"/> object into the underlying <see cref="IUnit"/> implementation
        /// </summary>
        /// <param name="bundle"></param>
        void In(Bundle bundle);

        /// <summary>
        /// Gets or sets the successor (next hop)
        /// </summary>
        IUnit Successor { get; set; }
    }
}
