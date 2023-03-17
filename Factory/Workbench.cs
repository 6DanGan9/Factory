using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Workbench
    {
        public string Name { get; private set; }
        public double WorkBoost { get; private set; }

        public Workbench() { }

        public Workbench(ExcelHelper excel, int row)
        {
            Name = excel.Get(row, 1);
            WorkBoost = Convert.ToDouble(excel.Get(row, 2));
        }
    }
}
