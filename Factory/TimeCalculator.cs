using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal static class TimeCalculator
    {
        public static TimeSpan CalcTimeToComplite(Task task, List<Worker> workers, Workbench workbench)
        {
            TimeSpan timeToCompliteTask;
            int neededWorkersCount = 0;
            double summEfficiency = 0;
            double summProgress = 0;
            var sortedWorkers = workers.OrderBy(x => x.LastTime);
            workers.Clear();
            foreach (var worker in sortedWorkers)
                workers.Add(worker);
            for (int i = 0; i < workers.Count - 1; i++)
            {
                summEfficiency += workers[i].Efficiency * workbench.WorkBoost;
                summProgress += summEfficiency * (TimeToWork(workers[i], workers[i + 1])).TotalHours;
                if (summProgress > task.LaborIntensity)
                {
                    neededWorkersCount = i;
                    break;
                }
            }
            if (neededWorkersCount > 0)
            {
                timeToCompliteTask = TimeToWork(workers[0], workers[neededWorkersCount]) - TimeSpan.FromHours((summProgress - task.LaborIntensity) / summEfficiency);
            }
            else
            {
                timeToCompliteTask = TimeSpan.FromHours((task.LaborIntensity - summProgress) / summEfficiency);
            }
            return timeToCompliteTask;
        }

        private static TimeSpan TimeToWork (Worker worker1, Worker worker2)
        {
            if (worker1.LastTime.Day == worker2.LastTime.Day)
                return worker2.LastTime - worker1.LastTime;
            else
            {
                int days = 0;
                DateTime Day1 = new();
                foreach (var day in worker1.Dates)
                    if (worker1.LastTime.Day == day.Day)
                        Day1 = day;
                TimeSpan timeToEndDay = Day1 + Factory.WorkingDayLength - worker1.LastTime;
                DateTime Day2 = new();
                foreach (var day in worker2.Dates)
                    if (worker1.LastTime.Day == day.Day)
                        Day2 = day;
                TimeSpan timeFromStartDay = worker2.LastTime - Day2;
                days = worker2.LastTime.Day - worker1.LastTime.Day - 1;
                TimeSpan timeToWork = timeToEndDay + (days * Factory.WorkingDayLength) + timeFromStartDay;
                return timeToWork;
            }
        }
    }
}
