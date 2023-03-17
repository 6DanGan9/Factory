using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class TaskEventDescriptor : EventArgs
    {
        public Task Task { get; set; }
    }
}
