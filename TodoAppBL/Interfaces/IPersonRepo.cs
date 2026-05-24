using TodoAppBL.Models;

namespace TodoAppBL.Interfaces
{
    public interface IPersonRepo
    {
        void Add(Person person);

        void Update(Person person);

        void DeleteById(string id);

        List<Person> GetAll();

        Person? GetById(string id);

        bool Exists(string id);
    }
}