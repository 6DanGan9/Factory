using Factory.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Agents
{
    internal class WorkbenchAgent : IAgent
    {
        public Queue<Massage> MassBox { get; set; }

        private Workbench Workbench { get; set; }
        public double WorkBoost { get { return Workbench.WorkBoost; } }
        public DateTime LastTime { get { return Tasks.Last().EndTime; } }
        public List<TaskAgent> Tasks = new() { Factory.StartTask };

        public WorkbenchAgent(Workbench workbench)
        {
            Workbench = workbench;
            MassBox = new();
        }
        /// <summary>
        /// Принимает таску для выполнения.
        /// </summary>
        internal void AcceptTask(TaskAgent task)
        {
            Tasks.Add(task);
        }

        internal void GetInfo()
        {
            Console.Write($"{Workbench.Name}: ");
            for (int i = 1; i < Tasks.Count; i++)
            {
                Console.Write($" [{Tasks[i].Number}] ({Tasks[i].StartTime} - {Tasks[i].EndTime})");
            }
            Console.WriteLine("\n");
        }

        internal void CheckMassBox()
        {
            while (MassBox.Count > 0)
            {
                Massage massage = MassBox.Dequeue();
                switch (massage.Command)
                {
                    case "CanWork":
                        var name = (string)massage.Obj;
                        if (name == Workbench.Name)
                        {
                            massage.From.MassBox.Enqueue(new Massage("Can", true, this, massage.From));
                        }
                        break;
                    case "AcceptTask":
                        AcceptTask((TaskAgent)massage.Obj);
                        break;
                }
            }
        }
    }
}
