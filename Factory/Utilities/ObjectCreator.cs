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
        public static void CreateTasksList()
        {
            Factory.StartTask.SetNumber(0);
            Factory.Tasks.Add(Factory.StartTask);
            ExcelHelper excel = new();
            excel.Open("Tasks");
            int row = 2;
            while (excel.Get(row, 1) != "")
            {
                Task task = new(excel, row);
                Factory.Tasks.Add(task);
                row++;
            }
            excel.Close();
            for (int i = 1; i < Factory.Tasks.Count - 1; i++)
                Factory.Tasks[i].Initialization();
        }

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
