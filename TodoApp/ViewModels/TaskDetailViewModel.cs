using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.ViewModels.Base;
using TodoApp.ViewModels.TaskDetail;
using TodoAppBL.Models;
using TodoAppBL.Services;
using TaskModel = TodoAppBL.Models.Task;

namespace TodoApp.ViewModels
{
    public class TaskDetailViewModel : ViewModel, IQueryAttributable
    {
        private TaskService TaskService { get; }
        private PersonService PersonService { get; }
        private NavigationService NavigationService { get; }

        private TaskModel _task = new();

        private string _pageTitle = "Nieuwe taak";
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                NotifyPropertyChanged();
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged();

                PageTitle = string.IsNullOrWhiteSpace(value)
                    ? "Nieuwe taak"
                    : value;
            }
        }

        private bool _isExistingTask;
        public bool IsExistingTask
        {
            get => _isExistingTask;
            set
            {
                _isExistingTask = value;
                NotifyPropertyChanged();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<PersonPickerViewModel> _persons = new();
        public ObservableCollection<PersonPickerViewModel> Persons
        {
            get => _persons;
            set
            {
                _persons = value;
                NotifyPropertyChanged();
            }
        }

        private PersonPickerViewModel? _selectedPerson;
        public PersonPickerViewModel? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
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

        public ICommand SaveCommand { get; init; }
        public ICommand CancelCommand { get; init; }

        public TaskDetailViewModel(
            TaskService taskService,
            PersonService personService,
            NavigationService navigationService)
        {
            TaskService = taskService;
            PersonService = personService;
            NavigationService = navigationService;

            SaveCommand = new Command(async () => await OnSaveAsync());
            CancelCommand = new Command(async () => await OnCancelAsync());

            LoadPersons();
            LoadTask(new TaskModel(), false);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            LoadPersons();

            if (query.TryGetValue("id", out var idValue))
            {
                var id = idValue.ToString();

                if (!string.IsNullOrWhiteSpace(id))
                {
                    var task = TaskService.GetById(id);

                    if (task != null)
                    {
                        LoadTask(task, true);
                        return;
                    }
                }
            }

            LoadTask(new TaskModel(), false);
        }

        private void LoadPersons()
        {
            var persons = PersonService.GetAll();

            Persons = new ObservableCollection<PersonPickerViewModel>(
                persons.Select(person => new PersonPickerViewModel(person))
            );
        }

        private void LoadTask(TaskModel task, bool isExistingTask)
        {
            _task = task;

            Title = task.Title;
            Description = task.Description;
            IsCompleted = task.IsCompleted;

            SelectedPerson = Persons.FirstOrDefault(person => person.Id == task.PersonId);

            IsExistingTask = isExistingTask;
            ErrorMessage = string.Empty;
        }

        private async System.Threading.Tasks.Task OnSaveAsync()
        {
            try
            {
                _task.Title = Title;
                _task.Description = Description;
                _task.IsCompleted = IsCompleted;
                _task.PersonId = SelectedPerson?.Id ?? string.Empty;

                if (IsExistingTask)
                {
                    TaskService.UpdateTask(_task);
                }
                else
                {
                    TaskService.AddTask(_task);
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
    }
}