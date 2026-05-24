using LiteDB;
using TodoAppBL.Interfaces;
using Task = TodoAppBL.Models.Task;

namespace TodoAppDL.Repositories
{
    public class LiteDbTaskRepository : ITaskRepo
    {
        private readonly LiteDatabaseConnection Connection;

        public LiteDbTaskRepository(LiteDatabaseConnection connection)
        {
            Connection = connection;
        }

        private ILiteCollection<Task> GetCollection()
        {
            return Connection.GetCollection<Task>();
        }

        public void Add(Task task)
        {
            GetCollection().Insert(task);
        }

        public void Update(Task task)
        {
            GetCollection().Update(task);
        }

        public void DeleteById(string id)
        {
            GetCollection().Delete(id);
        }

        public List<Task> GetAll()
        {
            return GetCollection().FindAll().ToList();
        }

        public Task? GetById(string id)
        {
            return GetCollection().FindById(id);
        }

        public bool Exists(string id)
        {
            return GetCollection().Exists(task => task.Id == id);
        }
    }
}