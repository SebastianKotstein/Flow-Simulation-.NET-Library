using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.model
{
    /// <summary>
    /// Represents a single <see cref="Entity"/>. Each <see cref="Entity"/> object has a unique identifier (see <see cref="Id"/>).
    /// </summary>
    public class Entity
    {
        private string _id;

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
    }
}
