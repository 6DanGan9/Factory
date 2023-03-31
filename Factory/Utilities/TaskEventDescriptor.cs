using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory.Objects;
using Task = Factory.Objects.Task;

namespace Factory.Utilities
{
    internal class TaskEventDescriptor : EventArgs
    {
        public Task Task { get; set; }
    }
}
