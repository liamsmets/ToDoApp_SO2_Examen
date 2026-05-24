using CommunityToolkit.Mvvm.Messaging;
using TodoAppBL.Interfaces;
using TodoAppBL.Messages;
using TodoAppBL.Models;

namespace TodoAppBL.Services
{
    public class PersonService
    {
        private IPersonRepo PersonRepo{ get; }

        public PersonService(IPersonRepo personRepo)
        {
            PersonRepo = personRepo;
        }

        public List<Person> GetAll()
        {
            return PersonRepo.GetAll();
        }

        public Person? GetById(string id)
        {
            return PersonRepo.GetById(id);
        }

        public void AddPerson(Person person)
        {
            ArgumentNullException.ThrowIfNull(person);

            Validate(person);

            var now = DateTime.Now;

            person.CreatedAt = now;
            person.UpdatedAt = now;
            PersonRepo.Add(person);
            

            WeakReferenceMessenger.Default.Send(new PersonSavedMessage(person));
        }

        public void UpdatePerson(Person person)
        {
            ArgumentNullException.ThrowIfNull(person);

            Validate(person);

            var now = DateTime.Now;

            person.UpdatedAt = now;
            PersonRepo.Update(person);
            
            WeakReferenceMessenger.Default.Send(new PersonSavedMessage(person));
        }

        public void DeleteById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            PersonRepo.DeleteById(id);

            WeakReferenceMessenger.Default.Send(new PersonDeletedMessage(id));
        }

        private void Validate(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FirstName))
                throw new InvalidDataException("Voornaam is verplicht.");
        }
    }
}