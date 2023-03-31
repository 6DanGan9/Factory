using Factory.Objects;
using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Factory.Objects.Task;

namespace Factory
{
    internal static class ObjectCreator
    {
        /// <summary>
        /// Считывает список тасок из Excel.
        /// </summary>
        public static void CreateTasksList()
        {
            List<Task> Tasks = new();
            Factory.StartTask.SetNumber(0);
            Tasks.Add(Factory.StartTask);
            ExcelHelper excel = new();
            excel.Open("Tasks");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Task task = new(excel, row);
                Tasks.Add(task);
                row++;
            }
            excel.Close();
            Factory.EndTask.SetNumber(int.MaxValue);
            Tasks.Add(Factory.EndTask);
            Factory.Tasks = new Task[Tasks.Count];
            Factory.Tasks = Tasks.ToArray();
            for (int i = 1; i < Tasks.Count - 1; i++)
                Tasks[i].Initialization();
        }
        /// <summary>
        /// Считывает список рабочих из Excel.
        /// </summary>
        public static void CreateWorkersList()
        {
            ExcelHelper excel = new();
            excel.Open("Workers");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Worker worker = new(excel, row);
                Factory.Workers.Add(worker);
                row++;
            }
            excel.Close();
        }
        /// <summary>
        /// Считывает список рабочих мест из Excel.
        /// </summary>
        public static void CreateWorkbenchesList()
        {
            ExcelHelper excel = new();
            excel.Open("Workbenches");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Workbench workbench = new(excel, row);
                Factory.Workbenches.Add(workbench);
                row++;
            }
            excel.Close();
        }
    }
}
