using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Objects
{
    internal class Specialization
    {
        public string Name { get; }
        public double Complexity { get; }

        public Specialization() { }

        public Specialization(string name, double complexity)
        {
            Name = name;
            Complexity = complexity;
        }
        /// <summary>
        /// Проверяет, подходит ли рабочий для таски.
        /// </summary>
        public bool CanUsed(Specialization spec)
        {
            if (spec.Name == Name && spec.Complexity >= Complexity)
                return true;
            else
                return false;
        }
    }
}
