using Factory.Interfaises;
using Factory.Objects;
using Factory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Agents
{
    internal class WorkerAgent : IAgent
    {
        public Queue<Massage> MassBox { get; set; }

        private Worker Worker { get; set; }
        /// <summary>
        /// Минимальное время, начиная с которого будет свободен.
        /// </summary>
        public DateTime LastTime { get { return Tasks.Last().EndTime; } }
        public List<TaskAgent> Tasks = new() { Main.Factory.StartTask };

        public WorkerAgent(Worker worker)
        {
            Worker = worker;
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
            Console.Write($"{Worker.Name}: ");
            for(int i = 1; i < Tasks.Count; i++)
            {
                Console.Write($" [{Tasks[i].Number}] ({Tasks[i].StartTime} - {Tasks[i].EndTime})");
            }
            Console.WriteLine("\n");
        }
        public void AddMassage(Massage massage)
        {
            MassBox.Enqueue(massage);
        }

        internal void CheckMassBox()
        {
            while (MassBox.Count > 0)
            {
                Massage massage = MassBox.Dequeue();
                switch(massage.Command)
                {
                    case "CanWork":
                        var specialization = (Specialization)massage.Obj;
                        if (specialization.CanUsed(Worker.Specialization))
                        {
                            massage.From.AddMassage(new Massage("Can", true, this, massage.From));
                        }
                        break;
                    case "CulcTimeToComplite":
                        var variant = (Variant)massage.Obj;
                        DateTime TimeOfEnd = TimeCalculator.CalcDateOfEndTask(variant, Worker.Efficiency, Worker.Dates);
                        massage.From.AddMassage(new Massage("TimeOfEnd", TimeOfEnd, this, massage.From));
                        break;
                    case "AcceptTask":
                        AcceptTask((TaskAgent)massage.Obj);
                        break;

                }
            }
        }
    }
}
