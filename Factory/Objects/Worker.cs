using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Objects
{
    internal class Worker
    {
        public string Name { get; private set; }
        public double Efficiency { get; private set; }
        public DateTime LastTime { get => LT(); }


        public Specialization Specialization { get; private set; }
        public List<TaskToWork> Tasks = new();
        public List<DateTime> Dates = new();

        public Worker() { }

        public Worker(ExcelHelper excel, int row)
        {
            Name = excel.Get(row, 1);
            Efficiency = Convert.ToDouble(excel.Get(row, 2));
            Specialization = new(excel.Get(row, 3), Convert.ToDouble(excel.Get(row, 4)));
            string dates = excel.Get(row, 5);
            string date;
            while (dates.Length > 0)
            {
                int number = dates.IndexOf(',');
                if (number > 0)
                {
                    date = dates.Substring(0, number);
                    date += "/03/2023 8:00:00";
                    dates = dates.Substring(number + 1);
                }
                else
                {
                    date = dates;
                    date += "/03/2023 8:00:00";
                    dates = "";
                }
                Dates.Add(DateTime.Parse(date));
            }
        }

        public void AddTask(TimeSpan workTime)
        {
            TaskToWork task = new(LastTime, DateEnd(LastTime, workTime));
            Tasks.Add(task);
        }

        private DateTime DateEnd(DateTime start, TimeSpan timeToWork)
        {
            for (int i = 0; i < Dates.Count; i++)
                if (Dates[i].Day == start.Day)
                {
                    if (Dates[i] + Factory.WorkingDayLength - start >= timeToWork)
                        return start + timeToWork;
                    else
                    {
                        timeToWork -= Dates[i] + Factory.WorkingDayLength - start;
                        while (timeToWork > TimeSpan.Zero)
                        {
                            if (timeToWork > Factory.WorkingDayLength)
                            {
                                i++;
                                timeToWork -= Factory.WorkingDayLength;
                            }
                            else
                                return Dates[i] + timeToWork;
                        }
                    }
                }
            return DateTime.MinValue;
        }

        private DateTime LT()
        {
            if (Tasks.Count == 0)
                return Dates[0];
            else
                return Tasks.Last().EndTime;
        }
    }
}
