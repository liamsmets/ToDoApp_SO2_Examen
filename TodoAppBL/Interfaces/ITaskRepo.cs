using Task = TodoAppBL.Models.Task;

namespace TodoAppBL.Interfaces
{
    public interface ITaskRepo
    {
        void Add(Task task);

        void Update(Task task);

        void DeleteById(string id);

        List<Task> GetAll();

        Task? GetById(string id);

        bool Exists(string id);
    }
}