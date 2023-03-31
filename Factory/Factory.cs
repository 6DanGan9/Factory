using Factory.Objects;
using Factory.Utilities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Task = Factory.Objects.Task;

namespace Factory
{
    internal static class Factory
    {
        public static DateTime StartDate = DateTime.Parse("1/03/2023 8:00:00");
        public static List<Worker> Workers = new();
        public static Task StartTask = new() { IsPlanned = true, EndTime = StartDate };
        public static Task EndTask = new();
        public static Task[] Tasks = Array.Empty<Task>();
        public static List<Workbench> Workbenches = new();
        public static TimeSpan WorkingDayLength = TimeSpan.FromHours(10);

        public static void CreateSchedule()
        {
            ObjectCreator.CreateTasksList();
            ObjectCreator.CreateWorkersList();
            ObjectCreator.CreateWorkbenchesList();
            Start();
        }


        private static void Start()
        {
            TaskAgent.TaskPlanning(EndTask);
        }
    }
}
