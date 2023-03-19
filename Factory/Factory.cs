using Excel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal static class Factory
    {
        public static List<Worker> Workers = new();
        private static Task StartTask = new();
        public static List<Task> Tasks = new();
        public static List<Workbench> Workbenches = new();
        public static TimeSpan WorkingDayLength = TimeSpan.FromHours(10);

        public static void CreateSchedule()
        {
            CreateTasksList();
            CreateWorkersList();
            CreateWorkbenchesList();
            Start();
        }

        private static void CreateTasksList()
        {
            StartTask.SetNumber(0);
            Tasks.Add(StartTask);
            ExcelHelper excel = new();
            excel.Open("Tasks");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Task task = new(excel, row);
                Tasks.Add(task);
                row++;
            }
            foreach (var task in Tasks)
                task.Initialization();
            excel.Close();
        }

        private static void CreateWorkersList()
        {
            ExcelHelper excel = new();
            excel.Open("Workers");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Worker worker = new(excel, row);
                Workers.Add(worker);
                row++;
            }
            excel.Close();
        }

        private static void CreateWorkbenchesList()
        {
            ExcelHelper excel = new();
            excel.Open("Workbenches");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Workbench workbench = new(excel, row);
                Workbenches.Add(workbench);
                row++;
            }
            excel.Close();
        }


        private static void Start()
        {
            StartTask.Start();
        }
    }
}
