using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Pages;
using TodoApp.ViewModels.Base;
using TodoApp.ViewModels.PersonList;
using TodoAppBL.Messages;
using TodoAppBL.Models;
using TodoAppBL.Services;

namespace TodoApp.ViewModels
{
    public class PersonListViewModel : ViewModel
    {
        public PersonService PersonService { get; }
        public NavigationService NavigationService { get; }

        private ObservableCollection<PersonViewModel> _items = new();
        public ObservableCollection<PersonViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand AddPersonCommand { get; init; }
        public ICommand OpenPersonCommand { get; init; }

        public PersonListViewModel(PersonService personService, NavigationService navigationService)
        {
            PersonService = personService;
            NavigationService = navigationService;

            AddPersonCommand = new Command(async () => await OnAddPersonAsync());
            OpenPersonCommand = new Command(async item => await OnOpenPersonAsync(item));

            LoadPersons();

            WeakReferenceMessenger.Default.Register<PersonSavedMessage>(this, (recipient, message) =>
            {
                UpdatePersonInList(message.Person);
            });

            WeakReferenceMessenger.Default.Register<PersonDeletedMessage>(this, (recipient, message) =>
            {
                RemovePersonFromList(message.Id);
            });
        }

        private void LoadPersons()
        {
            var persons = PersonService.GetAll();

            Items = new ObservableCollection<PersonViewModel>(
                persons.Select(ConvertToViewModel)
            );
        }

        private PersonViewModel ConvertToViewModel(Person person)
        {
            return new PersonViewModel(person);
        }

        private void UpdatePersonInList(Person person)
        {
            var existingItem = Items.FirstOrDefault(item => item.Id == person.Id);

            if (existingItem == null)
                Items.Add(ConvertToViewModel(person));
            else
                existingItem.UpdateFrom(person);
        }

        private void RemovePersonFromList(string id)
        {
            var existingItem = Items.FirstOrDefault(item => item.Id == id);

            if (existingItem != null)
                Items.Remove(existingItem);
        }

        private async System.Threading.Tasks.Task OnAddPersonAsync()
        {
            await NavigationService.GoToAsync(nameof(PersonDetailPage));
        }

        private async System.Threading.Tasks.Task OnOpenPersonAsync(object item)
        {
            if (item is not PersonViewModel person)
                return;

            await NavigationService.GoToAsync(nameof(PersonDetailPage), new ShellNavigationQueryParameters
            {
                { "id", person.Id }
            });
        }
    }
}