using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Worker
    {
        public string Name { get; private set; }
        public double Efficiency { get; private set; }
        public Specialization Specialization { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public List<TaskToWork> Tasks = new();





    }
}
