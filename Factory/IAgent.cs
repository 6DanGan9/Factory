using Factory.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal interface IAgent
    {
        public Queue<Massage> MassBox { get; set; }
        public void CheckMassBox() 
        {

        }
    }
}
