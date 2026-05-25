using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Pages;
using TodoApp.ViewModels.Base;
using TodoApp.ViewModels.TaskList;
using TodoAppBL.Messages;
using TodoAppBL.Services;
using TodoAppBL.Strategies;
using TaskModel = TodoAppBL.Models.Task;

namespace TodoApp.ViewModels
{
    public class TaskListViewModel : ViewModel
    {
        private TaskService TaskService { get; }
        private NavigationService NavigationService { get; }

        public ObservableCollection<ITaskListStrategy> Strategies { get; }

        private ITaskListStrategy CurrentStrategy { get; set; }

        private ObservableCollection<TaskViewModel> _items = new();
        public ObservableCollection<TaskViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ApplyStrategyCommand { get; init; }

        public ICommand AddTaskCommand { get; init; }
        public ICommand OpenTaskCommand { get; init; }
        public ICommand OpenPersonsCommand { get; init; }

        public TaskListViewModel(
            TaskService taskService,
            NavigationService navigationService,
            IEnumerable<ITaskListStrategy> strategies)
        {
            TaskService = taskService;
            NavigationService = navigationService;

            Strategies = new ObservableCollection<ITaskListStrategy>(strategies);
            CurrentStrategy = Strategies.First();

            ApplyStrategyCommand = new Command<ITaskListStrategy>(ApplyStrategy);

            AddTaskCommand = new Command(async () => await OnAddTaskAsync());
            OpenTaskCommand = new Command(async item => await OnOpenTaskAsync(item));
            OpenPersonsCommand = new Command(async () => await OnOpenPersonsAsync());

            LoadTasks();

            WeakReferenceMessenger.Default.Register<TaskSavedMessage>(this, (recipient, message) =>
            {
                UpdateTaskInList(message.Task);
            });
        }

        private void ApplyStrategy(ITaskListStrategy? strategy)
        {
            if (strategy == null)
                return;

            CurrentStrategy = strategy;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = TaskService.GetAll();
            var filteredTasks = CurrentStrategy.Apply(tasks);

            Items = new ObservableCollection<TaskViewModel>(
                filteredTasks.Select(ConvertToViewModel)
            );
        }

        private TaskViewModel ConvertToViewModel(TaskModel task)
        {
            return new TaskViewModel(task, OnTaskCompletedChanged);
        }

        private void OnTaskCompletedChanged(TaskViewModel taskViewModel, bool isCompleted)
        {
            TaskService.ToggleCompleted(taskViewModel.Id, isCompleted);
        }

        private void UpdateTaskInList(TaskModel task)
        {
            var shouldBeVisible = CurrentStrategy
                .Apply(new List<TaskModel> { task })
                .Any();

            var existingItem = Items.FirstOrDefault(item => item.Id == task.Id);

            if (!shouldBeVisible)
            {
                if (existingItem != null)
                    Items.Remove(existingItem);

                return;
            }

            if (existingItem == null)
            {
                Items.Add(ConvertToViewModel(task));
            }
            else
            {
                existingItem.UpdateFrom(task);
            }

            if (CurrentStrategy.RequiresReorder)
                LoadTasks();
        }

        private async Task OnAddTaskAsync()
        {
            await NavigationService.GoToAsync(nameof(TaskDetailPage));
        }

        private async Task OnOpenTaskAsync(object? item)
        {
            if (item is not TaskViewModel task)
                return;

            await NavigationService.GoToAsync(nameof(TaskDetailPage), new ShellNavigationQueryParameters
            {
                { "id", task.Id }
            });
        }

        private async Task OnOpenPersonsAsync()
        {
            await NavigationService.GoToAsync(nameof(PersonListPage));
        }
    }
}