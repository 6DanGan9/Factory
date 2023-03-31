using Factory.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Factory.Objects.Task;

namespace Factory
{
    internal static class TaskAgent
    {
        public static void TaskPlanning(Task task)
        {
            if(task.CanStartPlanning())
                task.StartPlanning();
            else
            {
                foreach(var subtask in task.NeededTasks)
                {
                    if (subtask.CanStartPlanning())
                        subtask.StartPlanning();
                    else
                        TaskPlanning(subtask);
                }
            }
            foreach (var subtask in task.NeededTasks)
            {
                subtask.FreeTime = task.StartTime - subtask.EndTime;
            }
        }
    }
}
