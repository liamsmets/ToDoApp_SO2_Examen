using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Strategies
{
    public class UnfinishedTasksStrategy : ITaskListStrategy
    {
        public string Name => "Toon niet afgewerkt";
        public bool RequiresReorder => false;

        public List<Task> Apply(List<Task> tasks)
        {
            return tasks.Where(task => !task.IsCompleted).ToList();
        }
    }
}