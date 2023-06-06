using Factory.Objects;
using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Factory.Objects.Task;
using TaskAgent = Factory.Agents.TaskAgent;
using WorkerAgent = Factory.Agents.WorkerAgent;
using WorkbenchAgent = Factory.Agents.WorkbenchAgent;

namespace Factory
{
    internal static class ObjectCreator
    {
        public static Task StartTask = new() { EndTime = Main.Factory.StartDate };
        public static Task EndTask = new();
        /// <summary>
        /// Считывает список тасок из Excel.
        /// </summary>
        public static void CreateTasksList()
        {
            List<TaskAgent> Tasks = new();
            StartTask.SetNumber(0);
            Tasks.Add(new TaskAgent(StartTask) { IsPlanned = true });
            ExcelHelper excel = new();
            excel.Open("Tasks");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Task task = new(excel, row);
                Tasks.Add(new TaskAgent(task));
                row++;
            }
            excel.Close();
            EndTask.SetNumber(int.MaxValue);
            Tasks.Add(new TaskAgent(EndTask));
            Main.Factory.Tasks = new TaskAgent[Tasks.Count];
            Main.Factory.Tasks = Tasks.ToArray();
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
                Main.Factory.Workers.Add(new WorkerAgent(worker));
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
                Main.Factory.Workbenches.Add(new WorkbenchAgent(workbench));
                row++;
            }
            excel.Close();
        }
    }
}
