using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class TaskToWork
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TaskToWork() { }

        public TaskToWork(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
