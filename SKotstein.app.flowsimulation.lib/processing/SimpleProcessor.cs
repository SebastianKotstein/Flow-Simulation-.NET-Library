using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skotstein.app.flowsimulation.lib.processing
{
    /// <summary>
    /// Represents a simple processor where the processing of <see cref="Bundle"/> is fully controlled by the underlying <see cref="Worker"/>s
    /// </summary>
    public class SimpleProcessor : Processor
    {
        public override void Update(long tick)
        {
            foreach(Worker w in Worker)
            {
                w.Update(tick);
            }
        }

        public override int Count
        {
            get
            {
                int sum = 0;
                foreach(Worker w in Worker)
                {
                    sum += w.Count;
                }
                return sum;
            }
        }
        public override int EntityCount
        {
            get
            {

                int sum = 0;
                foreach (Worker w in Worker)
                {
                    sum += w.EntityCount;
                }
                return sum;
            }
        }
    }
}
