using Factory.Utilities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Objects
{
    internal class Task
    {
        public string Name { get; set; }
        public int Number { get; private set; }
        /// <summary>
        /// Объём работ.
        /// </summary>
        public double LaborIntensity { get; private set; }
        /// <summary>
        /// Максимальное кол-во рабочих на таске.
        /// </summary>
        public int MaxCountWorkers { get; private set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Время, на которое может сместиться таска.
        /// </summary>
        public TimeSpan FreeTime { get; set; }
        //необходимая специальность и квалифицированность рабочего.
        public Specialization NeededSpecialization = new();
        public string NeededWorkdench { get; set; }

        public List<int> NumbersNeededTasks = new();
        public List<int> NumbersNextTasks = new();

        public Task()
        {
        }
        public Task(ExcelHelper excel, int row)
        {
            Name = excel.Get(row, 1);
            Number = Convert.ToInt32(excel.Get(row, 2));
            MaxCountWorkers = Convert.ToInt32(excel.Get(row, 3));
            NeededWorkdench = excel.Get(row, 4);
            LaborIntensity = Convert.ToDouble(excel.Get(row, 5));
            NeededSpecialization = new(excel.Get(row, 6), Convert.ToDouble(excel.Get(row, 7)));
            string neededTasks = excel.Get(row, 8);
            int taskNumber;
            while (!string.IsNullOrEmpty(neededTasks))
            {
                int number = neededTasks.IndexOf(',');
                if (number > 0)
                {
                    taskNumber = Convert.ToInt32(neededTasks.Substring(0, number));
                    neededTasks = neededTasks.Substring(number + 1);
                }
                else
                {
                    taskNumber = Convert.ToInt32(neededTasks);
                    neededTasks = string.Empty;
                }
                NumbersNeededTasks.Add(taskNumber);
            }
            string nextTasks = excel.Get(row, 9);
            while (!string.IsNullOrEmpty(nextTasks))
            {
                int number = nextTasks.IndexOf(',');
                if (number > 0)
                {
                    taskNumber = Convert.ToInt32(nextTasks.Substring(0, number));
                    nextTasks = nextTasks.Substring(number + 1);
                }
                else
                {
                    taskNumber = Convert.ToInt32(nextTasks);
                    nextTasks = string.Empty;
                }
                NumbersNextTasks.Add(taskNumber);
            }
        }

        internal void SetNumber(int number)
        {
            Number = number;
        }

        internal void GetInfo()
        {
            Console.WriteLine($"[{Number}]{Name} ({StartTime} - {EndTime})\n");
        }
    }
}
