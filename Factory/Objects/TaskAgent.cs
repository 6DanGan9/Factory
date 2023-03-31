using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Task = Factory.Objects.Task;

namespace Factory.Objects
{
    internal static class TaskAgent
    {
        public static void TaskPlanning(Task task)
        {
            if (task.CanStartPlanning())
                StartPlanning(task);
            else
            {
                foreach (var subtask in task.NeededTasks)
                    TaskPlanning(subtask);
                StartPlanning(task);
            }
            foreach (var subtask in task.NeededTasks)
            {
                subtask.FreeTime = task.StartTime - subtask.EndTime;
            }
        }

        public static void StartPlanning(Task task)
        {
            if (task.Number == int.MaxValue)
                return;
            List<Worker> workers = CreateWorkersList(task);
            Workbench workbench = SearchWorkbench(task);
            task.Worker = workers[0];
            DateTime minDate = TimeCalculator.CalcDateOfEndTask(task, workers[0], workbench);
            for (int i = 1; i < workers.Count; i++)
            {
                DateTime time = TimeCalculator.CalcDateOfEndTask(task, workers[i], workbench);
                if (time < minDate)
                {
                    task.Worker = workers[i];
                }
            }
            task.EndTime = minDate;
            task.Worker.AcceptTask(task);
            workbench.AcceptTask(task);
            Console.WriteLine($"{task.Name}, время выполнения ({task.StartTime} - {task.EndTime}) будет выполнять {task.Worker.Name} на рабочем столе:{task.NeededWorkdench}.\n");
            foreach(var subtask in task.NeededTasks)
            {
                subtask.FreeTime = task.StartTime - subtask.EndTime;
            }
            task.IsPlanned = true;
        }

        private static List<Worker> CreateWorkersList(Task task)
        {
            List<Worker> workers = new();
            foreach (var worker in Factory.Workers)
            {
                if (task.NeededSpecialization.CanUsed(worker.Specialization))
                    workers.Add(worker);
            }
            return workers;
        }

        private static Workbench SearchWorkbench(Task task)
        {
            foreach (var workbench in Factory.Workbenches)
            {
                if (task.NeededWorkdench == workbench.Name)
                    return workbench;
            }
            return null;
        }
    }
}
