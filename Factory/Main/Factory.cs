using Factory.Agents;
using Factory.Objects;
using Factory.Utilities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Task = Factory.Objects.Task;

namespace Factory.Main
{
    internal static class Factory
    {
        public static DateTime StartDate = DateTime.Parse("1/03/2023 8:00:00");
        public static TaskAgent[] Tasks;
        public static TaskAgent StartTask => Tasks.First();
        public static TaskAgent EndTask => Tasks.Last();
        public static List<WorkerAgent> Workers = new();
        public static List<WorkbenchAgent> Workbenches = new();
        public static TimeSpan WorkingDayLength = TimeSpan.FromHours(10);

        public static void CreateSchedule()
        {
            ObjectCreator.CreateTasksList();
            ObjectCreator.CreateWorkersList();
            ObjectCreator.CreateWorkbenchesList();
            Start();
            GetInfo();
        }

        private static void Start()
        {
            Tasks.Last().TaskPlanning();
        }

        private static void GetInfo()
        {
            Console.WriteLine("Информация о тасках:");
            for (int i = 1; i < Tasks.Length - 1; i++)
            {
                Tasks[i].GetInfo();
            }
            Console.WriteLine("==========================================================================================================================");
            Console.WriteLine("Информация о рабочих:");
            foreach (var worker in Workers)
            {
                worker.GetInfo();
            }
            Console.WriteLine("==========================================================================================================================");
            Console.WriteLine("Информация о рабочих столах:");
            foreach (var workbench in Workbenches)
            {
                workbench.GetInfo();
            }
        }
    }
}
