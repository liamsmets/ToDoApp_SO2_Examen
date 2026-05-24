using LiteDB;
using TodoAppBL.Interfaces;
using TodoAppBL.Models;

namespace TodoAppDL.Repositories
{
    public class LiteDbPersonRepository : IPersonRepo
    {
        private readonly LiteDatabaseConnection Connection;

        public LiteDbPersonRepository(LiteDatabaseConnection connection)
        {
            Connection = connection;
        }

        private ILiteCollection<Person> GetCollection()
        {
            return Connection.GetCollection<Person>();
        }

        public void Add(Person person)
        {
            GetCollection().Insert(person);
        }

        public void Update(Person person)
        {
            GetCollection().Update(person);
        }

        public void DeleteById(string id)
        {
            GetCollection().Delete(id);
        }

        public List<Person> GetAll()
        {
            return GetCollection().FindAll().ToList();
        }

        public Person? GetById(string id)
        {
            return GetCollection().FindById(id);
        }

        public bool Exists(string id)
        {
            return GetCollection().Exists(person => person.Id == id);
        }
    }
}