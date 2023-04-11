using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Objects
{
    internal class Variant
    {
        public double LaborIntensity { get; private set; }
        public DateTime StartTime { get; set; }
        public double WorkBoost { get; set; }           
        public Variant(double laborIntensity, DateTime startTime, double workBoost)
        {
            LaborIntensity = laborIntensity;
            StartTime = startTime;
            WorkBoost = workBoost;
        }
    }
}
