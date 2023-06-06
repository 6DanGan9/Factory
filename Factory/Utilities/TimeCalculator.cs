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
        /// <summary>
        /// Возвращает момент времени, в который рабочий сможет выполнить заказ.
        /// </summary>
        public static DateTime CalcDateOfEndTask(Variant variant, double efficiency, List<DateTime> dates)
        {
            TimeSpan timeToCompliteTask;
                timeToCompliteTask = TimeSpan.FromHours(variant.LaborIntensity / (efficiency * variant.WorkBoost));
                return DateEndWork(variant.StartTime, timeToCompliteTask, dates);
        }
        /// <summary>
        /// Принимает время начала, длительность выполнения, и расписание рабочего, возвращает время, когда он закончит.
        /// </summary>
        private static DateTime DateEndWork(DateTime start, TimeSpan timeToWork, List<DateTime> Dates)
        {
            //Находим день, проверяем, успеет ли выполнить заказ рабочий в этот же день, если нет, то вычитаем этотдень и считаем пока на закончиться время.
            for (int i = 0; i < Dates.Count; i++)
                if (Dates[i].Day == start.Day)
                {
                    if (Dates[i] + Main.Factory.WorkingDayLength - start >= timeToWork)
                        return start + timeToWork;
                    else
                    {
                        timeToWork -= Dates[i] + Main.Factory.WorkingDayLength - start;
                        i++;
                        while (timeToWork > TimeSpan.Zero)
                        {
                            if (timeToWork > Main.Factory.WorkingDayLength)
                            {
                                i++;
                                timeToWork -= Main.Factory.WorkingDayLength;
                            }
                            else
                                return Dates[i] + timeToWork;
                        }
                    }
                }
            return DateTime.MinValue;
        }
    }
}
