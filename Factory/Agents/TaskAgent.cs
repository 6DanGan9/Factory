using Factory.Interfaises;
using Factory.Objects;
using Factory.Utilities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Task = Factory.Objects.Task;

namespace Factory.Agents
{
    internal class TaskAgent : IAgent
    {
        public Queue<Massage> MassBox { get; set; }

        private Task MainTask { get; set; }
        public DateTime StartTime { get { return MainTask.StartTime; } }
        public DateTime EndTime { get { return MainTask.EndTime; } }
        public int Number { get { return MainTask.Number; } }

        public List<TaskAgent> NeededTasks = new();
        public List<TaskAgent> NextTasks = new();

        private WorkerAgent MainWorker { get; set; }
        private WorkbenchAgent MainWorkbench { get; set; }

        public bool IsPlanned = false;

        public TaskAgent(Task task)
        {
            MainTask = task;
            MassBox = new();
        }


        /// <summary>
        /// Планирование заказа
        /// </summary>
        public void TaskPlanning()
        {
            //Если таска может начать планироваться, то планируем.
            if (CanStartPlanning())
                StartPlanning();
            else
            {
                //Планируем не запланированные педшествующие таски.
                foreach (var subtask in NeededTasks)
                    if (!subtask.IsPlanned)
                        subtask.TaskPlanning();
                StartPlanning();
            }
            //Считаем возможное смезение для предшествующих тасок.
            foreach (var subtask in NeededTasks)
            {
                subtask.CulcFreeTime(MainTask.StartTime);
            }
        }
        /// <summary>
        /// Алгоритм нахождения места для планирования.
        /// </summary>
        public void StartPlanning()
        {
            Console.WriteLine(MainTask.Name);
            //Если это закрывающая таска, значит планирование окончено.
            if (this == Main.Factory.EndTask)
            {
                CulcStartTime();
                //Считаем возможное смещение для предшествующих тасок.
                foreach (var subtask in NeededTasks)
                {
                    subtask.CulcFreeTime(MainTask.StartTime);
                }
                return;
            }
            List<WorkerAgent> workers = CreateWorkersList();
            if (workers.Count == 0)
            {
                Console.WriteLine("Workers hasn't been found");
                throw new Exception();
            }
            MainWorkbench = SearchWorkbench();
            if (MainWorkbench == null)
            {
                Console.WriteLine("Workbench hasn't been found");
                throw new Exception();
            }
            //Запрашиваем у рабочих, до какого времени они будут выполнять работу. 
            foreach (var worker in workers)
            {
                CulcStartTime();
                if (StartTime < MainWorkbench.LastTime)
                    MainTask.StartTime = MainWorkbench.LastTime;
                if (StartTime < worker.LastTime)
                    MainTask.StartTime = worker.LastTime;
                Variant variant = new(MainTask.LaborIntensity, StartTime, MainWorkbench.WorkBoost);
                worker.AddMassage(new Massage("CulcTimeToComplite", variant, this, worker));
                worker.CheckMassBox();
            }
            //Находим наименьшее время и выбераем рабочего.
            DateTime minTime = MassBox.Min(x => (DateTime)x.Obj);
            while (MassBox.Count > 0)
            {
                var mass = MassBox.Dequeue();
                if ((DateTime)mass.Obj == minTime)
                    MainWorker = (WorkerAgent)mass.From;
            }
            //Записываем время окончиания работы и добавляем таску к рабочему и столу.
            MainTask.EndTime = minTime;
            MainWorker.AddMassage(new Massage("AcceptTask", this, this, MainWorker));
            MainWorker.CheckMassBox();
            MainWorkbench.AddMassage(new Massage("AcceptTask", this, this, MainWorkbench));
            MainWorkbench.CheckMassBox();
            IsPlanned = true;
        }
        public void AddMassage(Massage massage)
        {
            MassBox.Enqueue(massage);
        }
        /// <summary>
        /// Считает возможное время для смещения.
        /// </summary>
        /// <param name="Time"></param>
        public void CulcFreeTime(DateTime Time)
        {
            MainTask.FreeTime = Time - MainTask.EndTime;
        }
        /// <summary>
        /// Проверяет, все ли предшествующие таски запланированы.
        /// </summary>
        public bool CanStartPlanning()
        {
            foreach (var task in NeededTasks)
            {
                if (!task.IsPlanned)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Начальная настройка таски создание перечня предшествующих и последующих тасок.
        /// </summary>
        public void Initialization()
        {
            if (MainTask.NumbersNeededTasks.Count == 0)
            {
                NeededTasks.Add(Main.Factory.StartTask);
                Main.Factory.StartTask.NextTasks.Add(this);
            }
            if (MainTask.NumbersNextTasks.Count == 0)
            {
                NextTasks.Add(Main.Factory.EndTask);
                Main.Factory.EndTask.NeededTasks.Add(this);
            }
            foreach (var task in Main.Factory.Tasks)
            {
                foreach (var number in MainTask.NumbersNeededTasks)
                {
                    if (task.Number == number)
                    {
                        NeededTasks.Add(task);
                    }
                }
                foreach (var number in MainTask.NumbersNextTasks)
                {
                    if (task.Number == number)
                    {
                        NextTasks.Add(task);
                    }
                }
            }
        }
        /// <summary>
        /// Создаёт список рабочих, способных выполнить таску.
        /// </summary>
        private List<WorkerAgent> CreateWorkersList()
        {
            List<WorkerAgent> workers = new();
            foreach (var worker in Main.Factory.Workers)
            {
                worker.AddMassage(new Massage("CanWork", MainTask.NeededSpecialization, this, worker));
                worker.CheckMassBox();
            }
            while (MassBox.Count > 0)
            {
                workers.Add((WorkerAgent)MassBox.Dequeue().From);
            }
            return workers;
        }
        /// <summary>
        /// Ищет рабочий стол для таски
        /// </summary>
        private WorkbenchAgent SearchWorkbench()
        {
            foreach (var workbench in Main.Factory.Workbenches)
            {
                workbench.MassBox.Enqueue(new Massage("CanWork", MainTask.NeededWorkdench, this, workbench));
                workbench.CheckMassBox();
            }
            return (WorkbenchAgent)MassBox.Dequeue().From;
        }
        /// <summary>
        /// Ищет наименьшее время, в которое может начаться выполнение.
        /// </summary>
        private void CulcStartTime()
        {
            var startTime = NeededTasks?.Max(x => x.EndTime) ?? Main.Factory.StartDate;
            MainTask.StartTime = startTime;
        }
        
        internal void GetInfo()
        {
            MainTask.GetInfo();
        }
    }
}
