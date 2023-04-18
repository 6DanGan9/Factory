using Factory.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Interfaises
{
    internal interface IAgent
    {
        public Queue<Massage> MassBox { get; set; }

        public void AddMassage(Massage massage)
        {
            MassBox.Enqueue(massage);
        }

        public void CheckMassBox()
        {

        }
    }
}
