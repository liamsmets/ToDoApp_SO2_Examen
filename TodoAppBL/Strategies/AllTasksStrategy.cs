using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Strategies
{
    public class AllTasksStrategy : ITaskListStrategy
    {
        public string Name => "Toon alles";

        public bool RequiresReorder => false;

        public List<Task> Apply(List<Task> tasks)
        {
            return tasks;
        }
    }
}