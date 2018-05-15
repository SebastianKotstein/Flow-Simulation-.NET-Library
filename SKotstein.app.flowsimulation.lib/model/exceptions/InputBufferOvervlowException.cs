using Skotstein.app.flowsimulation.lib.model;
using Skotstein.app.flowsimulation.lib.processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.model
{
    /// <summary>
    /// The <see cref="InputBufferOvervlowException"/> is thrown if the input buffer in <see cref="FilterSource"/> has reached its limit and is going to overflow
    /// </summary>
    public class InputBufferOvervlowException : Exception
    {
        private IFilter _source;
        private Bundle _bundle;

        /// <summary>
        /// Gets the <see cref="IFilter"/> object which is subject of this overflow
        /// </summary>
        public IFilter FilterSource
        {
            get
            {
                return _source;
            }

        }

        /// <summary>
        /// Gets the <see cref="Bundle"/> object causing this overflow
        /// </summary>
        public Bundle Bundle
        {
            get
            {
                return _bundle;
            }

            set
            {
                _bundle = value;
            }
        }

        /// <summary>
        /// Creates an <see cref="InputBufferOvervlowException"/> instance with the passed source and the passend bundle causing this overflow
        /// </summary>
        /// <param name="source"></param>
        /// <param name="bundle"></param>
        public InputBufferOvervlowException(IFilter source, Bundle bundle)
        {
            _source = source;
            _bundle = bundle;
        }
    }
}
