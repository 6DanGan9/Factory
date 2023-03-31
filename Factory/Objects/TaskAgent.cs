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
        /// <summary>
        /// Планирование заказа
        /// </summary>
        public static void TaskPlanning(Task task)
        {
            //Если таска может начать планироваться, то планируем.
            if (task.CanStartPlanning())
                StartPlanning(task);
            else
            {
                //Планируем не запланированные педшествующие таски.
                foreach (var subtask in task.NeededTasks)
                    if(!subtask.IsPlanned)
                    TaskPlanning(subtask);
                StartPlanning(task);
            }
            //Считаем возможное смезение для предшествующих тасок.
            foreach (var subtask in task.NeededTasks)
            {
                subtask.FreeTime = task.StartTime - subtask.EndTime;
            }
        }
        /// <summary>
        /// Алгоритм нахождения места для планирования.
        /// </summary>
        public static void StartPlanning(Task task)
        {
            //Если это закрывающая таска, значит планирование окончено.
            if (task == Factory.EndTask)
                return;
            List<Worker> workers = CreateWorkersList(task);
            Workbench workbench = SearchWorkbench(task);
            //Ищем рабочего, который раньше закончит выполнять таску.
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
            //Добавляем таску рабочему и столу.
            task.EndTime = minDate;
            task.Worker.AcceptTask(task);
            workbench.AcceptTask(task);
            Console.WriteLine($"{task.Name}, время выполнения ({task.StartTime} - {task.EndTime}) будет выполнять {task.Worker.Name} на рабочем столе:{task.NeededWorkdench}.\n");
            task.IsPlanned = true;
        }
        /// <summary>
        /// Создаёт список рабочих, способных выполнить таску.
        /// </summary>
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
        /// <summary>
        /// Ищет рабочий стол для таски
        /// </summary>
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
