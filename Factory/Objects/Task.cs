using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Objects
{
    internal class Task
    {
        public int Number { get; private set; }
        public double LaborIntensity { get; private set; }
        public int MaxCountWorkers { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan FreeTime { get; set; }
        public List<Task> NeededTasks = new();
        private List<int> NumbersNeededTasks = new();
        public List<Task> NextTasks = new();
        private List<int> NumbersNextTasks = new();
        public List<Task> ExpectedTasks = new();
        public Specialization NeededSpecialization = new();
        public string NeededWorkdench;

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
            string neededTasks = excel.Get(row, 8);
            int taskNumber;
            while (!string.IsNullOrEmpty(neededTasks))
            {
                int number = neededTasks.IndexOf(',');
                if (number > 0)
                {
                    taskNumber = Convert.ToInt32(neededTasks.Substring(0, number));
                    neededTasks = neededTasks.Substring(number + 1);
                }
                else
                {
                    taskNumber = Convert.ToInt32(neededTasks);
                    neededTasks = string.Empty;
                }
                NumbersNeededTasks.Add(taskNumber);
            }
            string nextTasks = excel.Get(row, 9);
            while (!string.IsNullOrEmpty(nextTasks))
            {
                int number = nextTasks.IndexOf(',');
                if (number > 0)
                {
                    taskNumber = Convert.ToInt32(nextTasks.Substring(0, number));
                    nextTasks = nextTasks.Substring(number + 1);
                }
                else
                {
                    taskNumber = Convert.ToInt32(nextTasks);
                    nextTasks = string.Empty;
                }
                NumbersNextTasks.Add(taskNumber);
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
            if (NumbersNeededTasks.Count == 0)
            {
                NeededTasks.Add(Factory.StartTask);
                Factory.StartTask.NextTasks.Add(this);
            }
            if (NumbersNextTasks.Count == 0)
            {
                NextTasks.Add(Factory.EndTask);
                Factory.EndTask.NeededTasks.Add(this);
            }
            foreach(var task in Factory.Tasks)
            {
                foreach(var number in NumbersNeededTasks)
                {
                    if (task.Number == number)
                    {
                        NeededTasks.Add(task);
                    }
                }
                foreach (var number in NumbersNextTasks)
                {
                    if (task.Number == number)
                    {
                        NextTasks.Add(task);
                    }
                }
            }
        }

        public bool CanStartPlanning()
        {
            if (ExpectedTasks.Count == 0)
                return true;
            else
                return false;
        }

        public void StartPlanning()
        {
            Console.WriteLine($"Task №{Number} start pnannig");

            List<Worker> workers = CreateWorkersList();
            List<Workbench> workbenches = CreateWorkbanchesList();
            var sortedWorkers = workers.OrderBy(x => x.LastTime);
            workers.Clear();
            foreach (var worker in sortedWorkers)
                workers.Add(worker);



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
