using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Strategies
{
    public class MostRecentlyUpdatedTasksStrategy : ITaskListStrategy
    {
        public string Name => "Alles, meest recente eerst";
        public bool RequiresReorder => true;

        public List<Task> Apply(List<Task> tasks)
        {
            return tasks.OrderByDescending(task => task.UpdatedAt).ToList();
        }
    }
}