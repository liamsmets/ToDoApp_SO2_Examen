using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Strategies
{
    public interface ITaskListStrategy
    {
        string Name { get; }

        bool RequiresReorder { get; }

        List<Task> Apply(List<Task> tasks);
    }
}