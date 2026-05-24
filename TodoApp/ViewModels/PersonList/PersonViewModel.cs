using TodoApp.ViewModels.Base;
using TodoAppBL.Models;

namespace TodoApp.ViewModels.PersonList
{
    public class PersonViewModel : ViewModel
    {
        public string Id { get; set; } = string.Empty;

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
            }
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                NotifyPropertyChanged();
            }
        }

        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                NotifyPropertyChanged();
            }
        }

        public PersonViewModel(Person person)
        {
            UpdateFrom(person);
        }

        public void UpdateFrom(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Age = person.Age;
        }
    }
}