using Skotstein.app.flowsimulation.lib.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.model
{
    /// <summary>
    /// The <see cref="HeaderNotFoundException"/> is thrown if a unknown header field is accessed.
    /// </summary>
    public class HeaderNotFoundException : Exception
    {
        private Bundle _subject;

        /// <summary>
        /// Gets or sets the <see cref="Bundle"/> being subject of this exception
        /// </summary>
        public Bundle Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        /// <summary>
        /// Creates a new instace of the <see cref="HeaderNotFoundException"/>
        /// </summary>
        /// <param name="subject">the <see cref="Bundle"/> being subject of this exception</param>
        public HeaderNotFoundException(Bundle subject)
        {
            _subject = subject;
        }
    }
}
