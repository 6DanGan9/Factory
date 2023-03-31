using Factory.SubObjects;
using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Objects
{
    internal class Workbench
    {
        public string Name { get; private set; }
        public double WorkBoost { get; private set; }
        public DateTime LastTime { get { return Tasks.Last().EndTime; } }
        public List<DateTime> Dates = new();
        public List<TaskToWork> Tasks = new();

        public Workbench() { }

        public Workbench(ExcelHelper excel, int row)
        {
            Name = excel.Get(row, 1);
            WorkBoost = Convert.ToDouble(excel.Get(row, 2));
            for (int i = 1; i <= 30; i++)
            {
                string date;
                date = Convert.ToString(i) + "/03/2023 8:00:00";
                Dates.Add(DateTime.Parse(date));
            }
            Tasks.Add(new(0, Dates[0], Dates[0]));
        }

        internal void AcceptTask(Task task)
        {
            TaskToWork subTask = new(task.Number, LastTime, task.EndTime);
            Tasks.Add(subTask);
        }
    }
}
