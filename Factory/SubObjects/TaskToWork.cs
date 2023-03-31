using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.SubObjects
{
    /// <summary>
    /// Вспомогательная таска для заполнения расписания ресурсов.
    /// </summary>
    internal class TaskToWork
    {
        public int TaskNumber { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TaskToWork() { }

        public TaskToWork(int number, DateTime startTime, DateTime endTime)
        {
            TaskNumber = number;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
