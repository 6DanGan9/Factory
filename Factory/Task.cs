using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    internal class Task
    {
        public int Number { get; private set; }
        public double LaborIntensity { get; private set; }
        public int MaxCountWorkers { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public List<Task> NeededTasks = new();
        private List<int> NumbersNeededTasks = new();
        public List<Task> ExpectedTasks = new();
        public Specialization NeededSpecialization = new();
        public string NeededWorkdench;


        public event EventHandler<TaskEventDescriptor> TaskStartPlanning;
        public event EventHandler<TaskEventDescriptor> TaskEndPlanning;

        public Task()
        {
        }
        public Task(ExcelHelper excel, int row)
        {
            Number = Convert.ToInt32(excel.Get(row, 2));
            MaxCountWorkers = Convert.ToInt32(excel.Get(row, 3));
            NeededWorkdench = excel.Get(row, 4);
            LaborIntensity = Convert.ToDouble(excel.Get(row, 5));
            NeededSpecialization = new(excel.Get(row, 6), Convert.ToDouble(excel.Get(row, 7)));
            string tasks = excel.Get(row, 8);
            int taskNumber;
            while (tasks.Length > 0)
            {
                int number = tasks.IndexOf(',');
                if (number > 0)
                {
                    taskNumber = Convert.ToInt32(tasks.Substring(0, number));
                    tasks = tasks.Substring(number + 1);
                }
                else
                {
                    taskNumber = Convert.ToInt32(tasks);
                    tasks = "";
                }
                NumbersNeededTasks.Add(taskNumber);
            }
        }

        internal void SetNumber(int number)
        {
            Number = number;
        }

        public void Start()
        {
            Console.WriteLine($"Task №{Number} has been pnanned");
            TaskEndPlanning.Invoke(this, new TaskEventDescriptor { Task = this });
        }

        public void Initialization()
        {
            foreach (var numbeTask in NumbersNeededTasks)
            {
                foreach (var task in Factory.Tasks)
                {
                    if (task.Number == numbeTask)
                    {
                        task.TaskEndPlanning += ExpectedTaskComplite;
                        NeededTasks.Add(task);
                        ExpectedTasks.Add(task);
                        break;
                    }
                }
            }

        }

        private void ExpectedTaskComplite(object? sender, TaskEventDescriptor e)
        {
            ExpectedTasks.Remove(e.Task);
            TryStartPlanning();
        }

        private void TryStartPlanning()
        {
            if (ExpectedTasks.Count == 0)
            {
                StartPlanning();
            }
        }

        private void StartPlanning()
        {
            Console.WriteLine($"Task №{Number} start pnannig");

            List<Worker> workers = CreateWorkersList();
            List<Workbench> workbenches = CreateWorkbanchesList();


            Console.WriteLine($"Task №{Number} has been pnanned");
            if (TaskEndPlanning != null)
            {
                TaskEndPlanning.Invoke(this, new TaskEventDescriptor { Task = this });
            }
        }

        private List<Worker> CreateWorkersList()
        {
            List<Worker> workers = new();
            foreach (var worker in Factory.Workers)
            {
                if (NeededSpecialization.CanUsed(worker.Specialization))
                    workers.Add(worker);
            }
            return workers;
        }

        private List<Workbench> CreateWorkbanchesList()
        {
            List<Workbench> workbenches = new();
            foreach (var workbench in Factory.Workbenches)
            {
                if (NeededWorkdench == workbench.Name)
                    workbenches.Add(workbench);
            }
            return workbenches;
        }
    }
}
