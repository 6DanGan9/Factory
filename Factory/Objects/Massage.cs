using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factory.Interfaises;

namespace Factory.Objects
{
    /// <summary>
    /// Класс сообщения.
    /// </summary>
    internal class Massage
    {
        public string Command { get; set; }
        public object Obj { get; set; }
        public IAgent From { get; set; }
        public IAgent To { get; set; }

        public Massage(string command, object obj, IAgent from, IAgent to)
        {
            Command = command;
            Obj = obj;
            From = from;
            To = to;
        }
    }
}
