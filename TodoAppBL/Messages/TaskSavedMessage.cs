using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Messages
{
    public class TaskSavedMessage
    {
        public Task Task { get; }

        public TaskSavedMessage(Task task)
        {
            Task = task;
        }
    }
}