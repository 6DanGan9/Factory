using Factory.Objects;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Factory.Objects.Task;

namespace Factory.Utilities
{
    internal static class TimeCalculator
    {
        public static DateTime DateEndWork(DateTime start, TimeSpan timeToWork, List<DateTime> Dates)
        {
            for (int i = 0; i < Dates.Count; i++)
                if (Dates[i].Day == start.Day)
                {
                    if (Dates[i] + Factory.WorkingDayLength - start >= timeToWork)
                        return start + timeToWork;
                    else
                    {
                        timeToWork -= Dates[i] + Factory.WorkingDayLength - start;
                        i++;
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
        public static DateTime CalcDateOfEndTask(Task task, Worker worker, Workbench workbench)
        {
            task.CalcStartTime();
            if (task.StartTime < worker.LastTime)
            {
                task.StartTime = worker.LastTime;
            }
            if(task.StartTime < workbench.LastTime)
            {
                task.StartTime = workbench.LastTime;
            }
            TimeSpan timeToCompliteTask;
            //int neededWorkersCount = 0;
            //double summEfficiency = 0;
            //double summProgress = 0;
            //else if (workers.Count == 1)
            //{
                timeToCompliteTask = TimeSpan.FromHours(task.LaborIntensity / (worker.Efficiency * workbench.WorkBoost));
                return DateEndWork(task.StartTime, timeToCompliteTask, worker.Dates);
            //}
            //for (int i = 0; i < workers.Count - 1; i++)
            //{
            //    summEfficiency += workers[i].Efficiency * workbench.WorkBoost;
            //    summProgress += summEfficiency * TimeToWork(workers[i], workers[i + 1]).TotalHours;
            //    if (summProgress > task.LaborIntensity)
            //    {
            //        neededWorkersCount = i;
            //        break;
            //    }
            //}
            //if (neededWorkersCount > 0)
            //{
            //    timeToCompliteTask = TimeToWork(workers[0], workers[neededWorkersCount]) - TimeSpan.FromHours((summProgress - task.LaborIntensity) / summEfficiency);
            //}
            //else
            //{
            //    timeToCompliteTask = TimeSpan.FromHours((task.LaborIntensity - summProgress) / summEfficiency);
            //}
            //return timeToCompliteTask;
        }

        //private static TimeSpan TimeToWork(Worker worker1, Worker worker2)
        //{
        //    if (worker1.TimeToStart.Day == worker2.TimeToStart.Day)
        //        return worker2.TimeToStart - worker1.TimeToStart;
        //    else
        //    {
        //        int days;
        //        DateTime Day1 = new();
        //        foreach (var day in worker1.Dates)
        //            if (worker1.TimeToStart.Day == day.Day)
        //                Day1 = day;
        //        TimeSpan timeToEndDay = Day1 + Factory.WorkingDayLength - worker1.TimeToStart;
        //        DateTime Day2 = new();
        //        foreach (var day in worker2.Dates)
        //            if (worker1.TimeToStart.Day == day.Day)
        //                Day2 = day;
        //        TimeSpan timeFromStartDay = worker2.TimeToStart - Day2;
        //        days = worker2.TimeToStart.Day - worker1.TimeToStart.Day - 1;
        //        TimeSpan timeToWork = timeToEndDay + days * Factory.WorkingDayLength + timeFromStartDay;
        //        return timeToWork;
        //    }
        //}
    }
}
