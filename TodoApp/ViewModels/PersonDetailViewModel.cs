using System.Windows.Input;
using TodoApp.ViewModels.Base;
using TodoAppBL.Models;
using TodoAppBL.Services;

namespace TodoApp.ViewModels
{
    public class PersonDetailViewModel : ViewModel, IQueryAttributable
    {
        public PersonService PersonService { get; }
        public NavigationService NavigationService { get; }

        private Person _person = new();

        private string _pageTitle = "Nieuwe persoon";
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                NotifyPropertyChanged();
            }
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                NotifyPropertyChanged();
                PageTitle = string.IsNullOrWhiteSpace(value) ? "Nieuwe persoon" : value;
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

        private DateTime _birthDate = DateTime.Today;
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                NotifyPropertyChanged();
            }
        }

        private string _imageUrl = string.Empty;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                NotifyPropertyChanged();
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isExistingPerson;
        public bool IsExistingPerson
        {
            get => _isExistingPerson;
            set
            {
                _isExistingPerson = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; init; }
        public ICommand CancelCommand { get; init; }
        public ICommand DeleteCommand { get; init; }

        public PersonDetailViewModel(PersonService personService, NavigationService navigationService)
        {
            PersonService = personService;
            NavigationService = navigationService;

            SaveCommand = new Command(async () => await OnSaveAsync());
            CancelCommand = new Command(async () => await OnCancelAsync());
            DeleteCommand = new Command(async () => await OnDeleteAsync());

            LoadPerson(new Person(), false);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var idValue))
            {
                var id = idValue.ToString();

                if (!string.IsNullOrWhiteSpace(id))
                {
                    var person = PersonService.GetById(id);

                    if (person != null)
                    {
                        LoadPerson(person, true);
                        return;
                    }
                }
            }

            LoadPerson(new Person(), false);
        }

        private void LoadPerson(Person person, bool isExistingPerson)
        {
            _person = person;

            FirstName = person.FirstName;
            LastName = person.LastName;
            BirthDate = person.BirthDate;
            ImageUrl = person.ImageUrl;

            IsExistingPerson = isExistingPerson;
            ErrorMessage = string.Empty;
        }

        private async System.Threading.Tasks.Task OnSaveAsync()
        {
            try
            {
                _person.FirstName = FirstName;
                _person.LastName = LastName;
                _person.BirthDate = BirthDate;
                _person.ImageUrl = ImageUrl;

                if (IsExistingPerson)
                {
                    PersonService.UpdatePerson(_person);
                }
                else
                {
                    PersonService.AddPerson(_person);
                }

                await NavigationService.GoToAsync("..");
            }
            catch (InvalidDataException exception)
            {
                ErrorMessage = exception.Message;
            }
        }

        private async System.Threading.Tasks.Task OnCancelAsync()
        {
            await NavigationService.ShowAlertAsync("Geannuleerd", "De wijzigingen werden niet opgeslagen.");
            await NavigationService.GoToAsync("..");
        }

        private async System.Threading.Tasks.Task OnDeleteAsync()
        {
            PersonService.DeleteById(_person.Id);

            await NavigationService.GoToAsync("..");
        }
    }
}